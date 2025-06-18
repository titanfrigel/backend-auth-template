using BackendAuthTemplate.API.Requests.Categories;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class CategoryControllerTests : ApiTestBase
    {
        [Fact]
        public async Task CreateCategory_Should_Return_ProductId()
        {
            await SetAdminJwtToken();

            CreateCategoryRequest request = CategoriesRequestsTestHelper.CreateCategoryRequest();

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/categories", request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            ReadCategoryDto? category = await response.Content.ReadFromJsonAsync<ReadCategoryDto>();

            _ = category.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllCategories_Should_Return_List()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/categories");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadCategoryDto>? categories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadCategoryDto>>();

            _ = categories.ShouldNotBeNull();
            categories.Items.ShouldAllBe(x => x.CreatedBy == null);
        }

        [Fact]
        public async Task GetAllCategories_IncludeCreatedBy_Should_FailForUsers()
        {

            HttpResponseMessage response = await _client.GetAsync("/api/v1/categories?include=createdBy");

            response.IsSuccessStatusCode.ShouldBeFalse();
        }

        [Fact]
        public async Task GetAllCategories_IncludeCreatedBy_Should_SucceedForAdmins()
        {
            await SetAdminJwtToken();

            HttpResponseMessage response = await _client.GetAsync("/api/v1/categories?include=createdBy");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadCategoryDto>? categories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadCategoryDto>>();

            _ = categories.ShouldNotBeNull();
            categories.Items.ShouldAllBe(x => x.CreatedBy != null);
        }

        [Fact]
        public async Task GetAllCategories_Include_Subcategories_ShouldReturnListOfCategories()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/categories?include=subcategories");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadCategoryDto>? categories = await response.Content.ReadFromJsonAsync<PaginatedList<ReadCategoryDto>>();

            _ = categories.ShouldNotBeNull();
            categories.Items.First().Subcategories.ShouldNotBeEmpty();
            categories.Items.First().Subcategories!.First().Category.ShouldBeNull();
        }

        [Fact]
        public async Task GetCategoryById_Should_Return_Category()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Category category = context.Categories.First();

            HttpResponseMessage response = await _client.GetAsync($"/api/v1/categories/{category.Id}");

            _ = response.EnsureSuccessStatusCode();

            ReadCategoryDto? readCategory = await response.Content.ReadFromJsonAsync<ReadCategoryDto>();

            _ = readCategory.ShouldNotBeNull();
            readCategory.Id.ShouldBe(category.Id);
        }
    }
}
