using BackendAuthTemplate.API.Requests.Subcategories;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Infrastructure.Identity;
using BackendAuthTemplate.Tests.Common.Categories;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class SubcategoryControllerTests : ApiTestBase
    {
        public async Task<Func<IDisposable>> TestScope1(IServiceScope scope)
        {
            IUser userContext = scope.ServiceProvider.GetRequiredService<IUser>();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser? user = await userManager.FindByEmailAsync("admin@example.com") ?? throw new Exception("User not found in the test database.");

            return () => userContext.BeginScope(user.Id);
        }

        public async Task<IDisposable> TestScope2(IServiceScope scope, IUser userContext)
        {
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser? user = await userManager.FindByEmailAsync("admin@example.com") ?? throw new Exception("User not found in the test database.");

            return userContext.BeginScope(user.Id);
        }

        public async Task<IDisposable> TestScope3(IServiceScope scope, IUser userContext)
        {
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser? user = await userManager.FindByEmailAsync("admin@example.com") ?? throw new Exception("User not found in the test database.");

            return Task.FromResult(userContext.BeginScope(user.Id));
        }

        public Task<IDisposable> TestScope4(IUser userContext, Guid id)
        {
            return Task.FromResult(userContext.BeginScope(id));
        }

        [Fact]
        public async Task CreateSubcategory_Should_Return_ProductId()
        {
            await SetAdminJwtToken();

            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Category category;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                category = await CategoriesEntitiesTestHelper.SeedCategory(context);
            }

            CreateSubcategoryRequest request = SubcategoriesRequestsTestHelper.CreateSubcategoryRequest(categoryId: category.Id);

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/subcategories", request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            ReadSubcategoryDto? subcategory = await response.Content.ReadFromJsonAsync<ReadSubcategoryDto>();

            _ = subcategory.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllSubcategories_Should_Return_List()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
        }
    }
}
