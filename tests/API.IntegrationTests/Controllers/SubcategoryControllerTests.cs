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
        public async Task GetSubcategoriesWithPagination_Should_Return_PaginatedList()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
            _ = subcategories.Items.ShouldNotBeNull();
            subcategories.Items.ShouldAllBe(x => x.CreatedBy == null);
        }

        [Fact]
        public async Task GetSubcategories_Sort_CategoryNameAscending_ShouldReturnListOfSubcategories()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Category categoryA;
            Subcategory subcategoryZ;
            Category categoryZ;
            Subcategory subcategoryA;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                categoryZ = await CategoriesEntitiesTestHelper.SeedCategory(context, entity: CategoriesEntitiesTestHelper.CreateValidCategory(name: "ZCategory"));
                subcategoryA = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context, entity: SubcategoriesEntitiesTestHelper.CreateValidSubcategory(name: "ASubcategory", categoryId: categoryZ.Id));
                categoryA = await CategoriesEntitiesTestHelper.SeedCategory(context, entity: CategoriesEntitiesTestHelper.CreateValidCategory(name: "ACategory"));
                subcategoryZ = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context, entity: SubcategoriesEntitiesTestHelper.CreateValidSubcategory(name: "ZSubcategory", categoryId: categoryA.Id));
            }

            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories?sorts[0].PropertyName=category.name&sorts[0].Direction=ascending");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
            _ = subcategories.Items.ShouldNotBeNull();
            subcategories.Items.ShouldAllBe(s => s.Category == null);
            subcategories.Items.First().Id.ShouldBe(subcategoryZ.Id);
        }

        [Fact]
        public async Task GetSubcategories_Sort_CategoryNameDescending_ShouldReturnListOfSubcategories()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Category categoryA;
            Subcategory subcategoryA;
            Category categoryZ;
            Subcategory subcategoryZ;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                categoryZ = await CategoriesEntitiesTestHelper.SeedCategory(context, entity: CategoriesEntitiesTestHelper.CreateValidCategory(name: "ZCategory"));
                subcategoryA = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context, entity: SubcategoriesEntitiesTestHelper.CreateValidSubcategory(name: "ASubcategory", categoryId: categoryZ.Id));
                categoryA = await CategoriesEntitiesTestHelper.SeedCategory(context, entity: CategoriesEntitiesTestHelper.CreateValidCategory(name: "ACategory"));
                subcategoryZ = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context, entity: SubcategoriesEntitiesTestHelper.CreateValidSubcategory(name: "ZSubcategory", categoryId: categoryA.Id));
            }

            HttpResponseMessage response = await _client.GetAsync("/api/v1/subcategories?sorts[0].PropertyName=category.name&sorts[0].Direction=descending");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadSubcategoryDto>? subcategories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadSubcategoryDto>>();

            _ = subcategories.ShouldNotBeNull();
            _ = subcategories.Items.ShouldNotBeNull();
            subcategories.Items.ShouldAllBe(s => s.Category == null);
            subcategories.Items.First().Id.ShouldBe(subcategoryA.Id);
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
