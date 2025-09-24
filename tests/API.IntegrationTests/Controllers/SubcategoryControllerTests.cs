using BackendAuthTemplate.API.Requests.Subcategories;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class SubcategoryControllerTests : ApiTestBase
    {
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
        public async Task GetAllSubcategoriesWithPagination_Should_Return_PaginatedList()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories/paginated");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllSubcategories_Should_Return_List()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories");

            _ = response.EnsureSuccessStatusCode();

            List<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<List<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetSubcategoryById_Should_Return_Subcategory()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Subcategory subcategory = context.Subcategories.First();

            HttpResponseMessage response = await _client.GetAsync($"/api/v1/subcategories/{subcategory.Id}");

            _ = response.EnsureSuccessStatusCode();

            ReadSubcategoryDto? readSubcategory = await response.Content.ReadFromJsonAsync<ReadSubcategoryDto>();

            _ = readSubcategory.ShouldNotBeNull();
            readSubcategory.Id.ShouldBe(subcategory.Id);
        }
    }
}
