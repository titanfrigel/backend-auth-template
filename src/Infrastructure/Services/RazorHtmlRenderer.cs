using BackendAuthTemplate.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.Extensions.Logging;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class RazorHtmlRenderer(IServiceProvider services, ILoggerFactory loggerFactory) : IHtmlRenderer
    {
        private readonly HtmlRenderer _htmlRenderer = new(services, loggerFactory);

        public async Task<string> RenderAsync<TComponent>(IDictionary<string, object?> dictionary, CancellationToken cancellationToken = default) where TComponent : IComponent
        {
            return await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                ParameterView parameters = ParameterView.FromDictionary(dictionary);
                HtmlRootComponent output = await _htmlRenderer.RenderComponentAsync<TComponent>(parameters);

                return output.ToHtmlString();
            });
        }
    }
}
