using Microsoft.AspNetCore.Mvc;
using System.Net;
using AuthTemplate.Controllers;

namespace AuthTemplate.Tests.Controllers
{
    public class TestControllerTests : IAsyncLifetime
    {
        private AuthWebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;
        private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

        public Task InitializeAsync()
        {
            _factory = new AuthWebApplicationFactory<Program>(_databaseName);
            _client = _factory.CreateClient();

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            _factory.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public void Test_WithController_ReturnsOk()
        {
            TestController controller = new();

            IActionResult result = controller.Test();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            string response = Assert.IsType<string>(okResult.Value);

            Assert.Equal("Test", response);
        }

        [Fact]
        public void TestAnonymous_WithController_ReturnsOk()
        {
            TestController controller = new();

            IActionResult result = controller.TestAnonymous();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            string response = Assert.IsType<string>(okResult.Value);

            Assert.Equal("TestAnonymous", response);
        }

        [Fact]
        public async Task Test_WithHttpRoute_ReturnsOk()
        {
            HttpResponseMessage response = await _client.GetAsync("/test/test");

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Test", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task TestAnonymous_WithHttpRoute_ReturnsOk()
        {
            HttpResponseMessage response = await _client.GetAsync("/test/testAnonymous");

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("TestAnonymous", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Test_ReturnsUnauthorized_WithoutToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _client.GetAsync("/test/test");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task TestAnonymous_Work_WithoutToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _client.GetAsync("/test/testAnonymous");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("TestAnonymous", await response.Content.ReadAsStringAsync());
        }
    }

}
