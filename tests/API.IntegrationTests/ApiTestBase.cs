using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace BackendAuthTemplate.API.IntegrationTests
{
    public abstract class ApiTestBase : IAsyncLifetime
    {
        protected CustomWebApplicationFactory<Program> _factory = null!;
        protected HttpClient _client = null!;

        public async Task InitializeAsync()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();

            string token = await _factory.GenerateUserJwtTokenAsync();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            _factory.Dispose();

            return Task.CompletedTask;
        }

        protected async Task<Func<IDisposable>> GetSeedingScopeAsAdmin()
        {
            return await _factory.GetScopedUserContextAs("admin@example.com");
        }

        protected void ClearJwtToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }

        protected async Task SetAdminJwtToken()
        {
            string token = await _factory.GenerateAdminJwtTokenAsync();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
