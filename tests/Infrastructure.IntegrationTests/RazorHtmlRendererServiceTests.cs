using BackendAuthTemplate.EmailComponents;
using BackendAuthTemplate.Infrastructure.Interfaces;
using BackendAuthTemplate.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Infrastructure.IntegrationTests
{
    public class RazorHtmlRendererServiceTests
    {
        private static IHtmlRenderer BuildHtmlRenderer()
        {
            ServiceCollection services = new();

            _ = services.AddLogging();
            _ = services.AddSingleton<IHtmlRenderer, RazorHtmlRenderer>();

            return services.BuildServiceProvider()
                           .GetRequiredService<IHtmlRenderer>();
        }

        [Fact]
        public async Task ResetPassword_Renders_Disclaimer_And_TimeLimit()
        {
            IHtmlRenderer renderer = BuildHtmlRenderer();

            string html = await renderer.RenderAsync<ResetPasswordTemplate>(new Dictionary<string, object?>
            {
                ["ResetUrl"] = "https://x/reset",
                ["ValidMinutes"] = 30
            }, CancellationToken.None);

            html.ShouldContain("adresse <strong>noreply</strong>");
            html.ShouldContain("30 minutes");
        }


        [Fact]
        public async Task ConfirmEmail_Renders_Disclaimer_And_TimeLimit_And_FirstName()
        {
            IHtmlRenderer renderer = BuildHtmlRenderer();

            string html = await renderer.RenderAsync<ConfirmEmailTemplate>(new Dictionary<string, object?>
            {
                ["FirstName"] = "Tom",
                ["ConfirmUrl"] = "https://x/reset",
                ["ValidHours"] = 4
            }, CancellationToken.None);

            html.ShouldContain("Bienvenue Tom");
            html.ShouldContain("adresse <strong>noreply</strong>");
            html.ShouldContain("4 heures");
        }
    }
}
