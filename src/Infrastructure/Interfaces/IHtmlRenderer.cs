using Microsoft.AspNetCore.Components;

namespace BackendAuthTemplate.Infrastructure.Interfaces
{
    public interface IHtmlRenderer
    {
        Task<string> RenderAsync<TComponent>(IDictionary<string, object?> dictionary, CancellationToken cancellationToken = default) where TComponent : IComponent;
    }
}
