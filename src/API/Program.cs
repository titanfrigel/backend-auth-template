using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Common.Middlewares;
using BackendAuthTemplate.API.Common.Swagger;
using BackendAuthTemplate.Application;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Infrastructure;
using BackendAuthTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

AuthSettings jwtSettings = builder.Configuration.GetSection("Auth").Get<AuthSettings>()!;
GeneralSettings generalSettings = builder.Configuration.GetSection("General").Get<GeneralSettings>()!;

if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("RemoteDevelopment"))
{
    _ = builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            Uri frontendUri = new(generalSettings.FrontendUri);

            _ = policy
                .WithOrigins(
                    "http://localhost",
                    generalSettings.FrontendUri
                )
                .SetIsOriginAllowed(origin =>
                {
                    Uri uri = new(origin);
                    return uri.Host == "localhost" || uri.Host == frontendUri.Host;
                })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}
else
{
    _ = builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            _ = policy
                .WithOrigins(generalSettings.FrontendUri)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower));
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddSignalR();

builder.Services.AddRateLimiter(options =>
    options.AddFixedWindowLimiter("auth", o =>
    {
        o.Window = TimeSpan.FromMinutes(1);
        o.PermitLimit = 5;
    })
);

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<ApiEntityOperationFilter>();
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.ValidAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                PathString path = context.HttpContext.Request.Path;

                // If the request is for the SignalR hub, read the token from the query string
                if (context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                {
                    StringValues accessToken = context.Request.Query["access_token"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new Error("Auth.Unauthorized", "Unauthorized", ErrorType.AccessUnAuthorized));
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new Error("Auth.Forbidden", "Forbidden", ErrorType.AccessForbidden));
            }
        };
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else if (app.Environment.IsEnvironment("RemoteDevelopment") || app.Environment.IsEnvironment("Testing"))
{
    await app.SeedDatabaseAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("RemoteDevelopment"))
{
    _ = app.UseSwagger(c =>
    {
        c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            string scheme = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? httpReq.Scheme;
            string basePath = httpReq.PathBase.HasValue ? httpReq.PathBase.Value : "/api";

            swaggerDoc.Servers =
            [
                new OpenApiServer { Url = $"{scheme}://{httpReq.Host}{basePath}" }
            ];
        });
    });
    _ = app.UseSwaggerUI(c =>
    {
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/api/swagger/{description.GroupName}/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} {description.GroupName.ToUpperInvariant()}");
        }
        c.RoutePrefix = "api/swagger";
    });
}
else
{
    _ = app.UseHsts();
}

app.UseRateLimiter();

app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UsePathBase("/api");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();

public partial class Program { }