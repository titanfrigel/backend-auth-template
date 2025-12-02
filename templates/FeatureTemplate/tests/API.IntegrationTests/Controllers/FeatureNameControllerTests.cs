using BackendAuthTemplate.API.Requests.FeatureName;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class FeatureNameControllerTests : ApiTestBase
    {
        [Fact]
        public async Task CreateEntityName_Should_Return_FeatureNameId()
        {
            await SetAdminJwtToken();

            CreateEntityNameRequest request = FeatureNameRequestsTestHelper.CreateEntityNameRequest();

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/FeatureName", request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            ReadEntityNameDto? entityName = await response.Content.ReadFromJsonAsync<ReadEntityNameDto>();

            _ = entityName.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetFeatureName_Should_Return_PaginatedList()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/FeatureName");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadEntityNameDto>? featureName = await response.Content.ReadFromJsonAsync<PaginatedList<ReadEntityNameDto>>();

            _ = featureName.ShouldNotBeNull();
            _ = featureName.Items.ShouldNotBeNull();
            featureName.Items.ShouldAllBe(x => x.CreatedBy == null);
        }


        [Fact]
        public async Task GetFeatureName_IncludeCreatedBy_Should_FailForUsers()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/FeatureName?includes=createdBy");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadEntityNameDto>? featureName = await response.Content.ReadFromJsonAsync<PaginatedList<ReadEntityNameDto>>();

            _ = featureName.ShouldNotBeNull();
            _ = featureName.Items.ShouldNotBeNull();
            featureName.Items.ShouldAllBe(x => x.CreatedBy == null);
        }

        [Fact]
        public async Task GetFeatureName_IncludeCreatedBy_Should_SucceedForAdmins()
        {
            await SetAdminJwtToken();

            HttpResponseMessage response = await _client.GetAsync("/api/v1/FeatureName?includes=createdBy");

            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadEntityNameDto>? featureName = await response.Content.ReadFromJsonAsync<PaginatedList<ReadEntityNameDto>>();

            _ = featureName.ShouldNotBeNull();
            _ = featureName.Items.ShouldNotBeNull();
            featureName.Items.ShouldAllBe(x => x.CreatedBy != null);
        }

        [Fact]
        public async Task GetFeatureName_SortNameAscending_Should_Return_PaginatedList()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            EntityName entityNameA;
            EntityName entityNameZ;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                entityNameA = await FeatureNameEntitiesTestHelper.SeedEntityName(context, entity: FeatureNameEntitiesTestHelper.CreateValidEntityName(name: "AFeatureName"));
                entityNameZ = await FeatureNameEntitiesTestHelper.SeedEntityName(context, entity: FeatureNameEntitiesTestHelper.CreateValidEntityName(name: "ZFeatureName"));
            }

            HttpResponseMessage response = await _client.GetAsync("/api/v1/FeatureName?sorts[0].PropertyName=name&sorts[0].Direction=ascending");
            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadEntityNameDto>? featureName = await response.Content.ReadFromJsonAsync<PaginatedList<ReadEntityNameDto>>();

            _ = featureName.ShouldNotBeNull();
            _ = featureName.Items.ShouldNotBeNull();
            featureName.Items.First().Id.ShouldBe(entityNameA.Id);
            featureName.Items.First().Name.ShouldBe(entityNameA.Name);
        }

        [Fact]
        public async Task GetFeatureName_SortNameDescending_Should_Return_PaginatedList()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            EntityName entityNameA;
            EntityName entityNameZ;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                entityNameA = await FeatureNameEntitiesTestHelper.SeedEntityName(context, entity: FeatureNameEntitiesTestHelper.CreateValidEntityName(name: "AFeatureName"));
                entityNameZ = await FeatureNameEntitiesTestHelper.SeedEntityName(context, entity: FeatureNameEntitiesTestHelper.CreateValidEntityName(name: "ZFeatureName"));
            }

            HttpResponseMessage response = await _client.GetAsync("/api/v1/FeatureName?sorts[0].PropertyName=name&sorts[0].Direction=descending");
            _ = response.EnsureSuccessStatusCode();

            PaginatedList<ReadEntityNameDto>? featureName = await response.Content.ReadFromJsonAsync<PaginatedList<ReadEntityNameDto>>();

            _ = featureName.ShouldNotBeNull();
            _ = featureName.Items.ShouldNotBeNull();
            featureName.Items.First().Id.ShouldBe(entityNameZ.Id);
            featureName.Items.First().Name.ShouldBe(entityNameZ.Name);
        }

        [Fact]
        public async Task GetEntityNameById_Should_Return_EntityName()
        {
            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            EntityName entityName;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);
            }

            HttpResponseMessage response = await _client.GetAsync($"/api/v1/FeatureName/{entityName.Id}");

            _ = response.EnsureSuccessStatusCode();

            ReadEntityNameDto? readEntiyName = await response.Content.ReadFromJsonAsync<ReadEntityNameDto>();

            _ = readEntiyName.ShouldNotBeNull();
            readEntiyName.Id.ShouldBe(entityName.Id);
        }

        [Fact]
        public async Task UpdateEntityName_Should_Return_NoContent()
        {
            await SetAdminJwtToken();

            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            EntityName entityName;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);
            }

            UpdateEntityNameRequest request = FeatureNameRequestsTestHelper.UpdateEntityNameRequest(name: "Updated Name");

            HttpResponseMessage response = await _client.PutAsJsonAsync($"/api/v1/FeatureName/{entityName.Id}", request);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteEntityName_Should_Return_NoContent()
        {
            await SetAdminJwtToken();

            using IServiceScope scope = _factory.ServiceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            EntityName entityName;
            Func<IDisposable> beginScope = await GetSeedingScopeAsAdmin();
            using (beginScope())
            {
                entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);
            }

            HttpResponseMessage response = await _client.DeleteAsync($"/api/v1/FeatureName/{entityName.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}
