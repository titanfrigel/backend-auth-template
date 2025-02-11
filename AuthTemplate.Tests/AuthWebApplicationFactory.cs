using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthTemplate.Dtos;

namespace AuthTemplate.Tests
{
    public class AuthWebApplicationFactory<TStartup>(string databaseName) : BaseWebApplicationFactory<TStartup>(databaseName) where TStartup : class
    {
        private string? _token;
        private bool _isRetrievingToken;

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            if (!_isRetrievingToken)
            {
                AddAuthHeaderAsync(client).GetAwaiter().GetResult();
            }
        }

        private async Task<string> GetAuthTokenAsync()
        {
            _isRetrievingToken = true;
            try
            {
                HttpClient client = CreateClient();

                HttpResponseMessage response = await client.PostAsJsonAsync("/auth/login", new AuthLoginDto { Email = "admin@example.com", Password = "AdminPass123!"});
                _ = response.EnsureSuccessStatusCode();

                AuthTokenReadDto? result = await response.Content.ReadFromJsonAsync<AuthTokenReadDto>();

                return result?.AccessToken ?? throw new InvalidOperationException("Failed to get token");
            }
            finally
            {
                _isRetrievingToken = false;
            }
        }

        private async Task AddAuthHeaderAsync(HttpClient client)
        {
            _token ??= await GetAuthTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}
