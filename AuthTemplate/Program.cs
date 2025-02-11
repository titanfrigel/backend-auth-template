using AuthTemplate;
using AuthTemplate.Db;
using AuthTemplate.Db.Models;
using AuthTemplate.Middlewares;
using AuthTemplate.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

const string swaggerVersion = "v0.1";
const string appName = "AuthTemplateApi";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(swaggerVersion, new OpenApiInfo
    {
        Title = appName,
        Version = swaggerVersion
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter Your token in the text input below.\nExample: 'abc123xyz'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add UserContext and DbContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContext>();
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")));
}

// Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _ = dbContext.Database.EnsureDeleted();
        _ = dbContext.Database.EnsureCreated();
    }

    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await SeedRolesAndAdmin.SeedRolesAndAdminAsync(roleManager, userManager);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger(c =>
    {
        c.RouteTemplate = "api/v1/swagger/{documentName}/swagger.json";
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            string scheme = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? httpReq.Scheme;
            string basePath = httpReq.PathBase.HasValue ? httpReq.PathBase.Value : "/api/v1";

            swaggerDoc.Servers =
            [
                new OpenApiServer { Url = $"{scheme}://{httpReq.Host}{basePath}" }
            ];
        });
    });
    _ = app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/api/v1/swagger/{swaggerVersion}/swagger.json", $"{appName} {swaggerVersion}");
        c.RoutePrefix = "api/v1/swagger";
    });
}
else
{
    _ = app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UsePathBase("/api/v1");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }