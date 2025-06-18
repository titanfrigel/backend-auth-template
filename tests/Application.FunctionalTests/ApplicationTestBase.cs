using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BackendAuthTemplate.Application.FunctionalTests
{
    public abstract class ApplicationTestBase : IAsyncLifetime
    {
        protected ApplicationTestFixture _fixture = null!;
        protected IMediator _mediator = null!;

        public Task InitializeAsync()
        {
            _fixture = new ApplicationTestFixture();
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
