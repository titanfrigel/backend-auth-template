using System.Security.Claims;

namespace AuthTemplate.Services
{
    public class UserContext(IHttpContextAccessor httpContextAccessor)
    {
        private Guid GetGuidValueOrEmpty(string claimType)
        {
            string? value = httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);

            _ = value!;
            return value != null ? Guid.Parse(value) : Guid.Empty;
        }

        public Guid UserId => GetGuidValueOrEmpty(ClaimTypes.NameIdentifier);

        public bool HasContext => httpContextAccessor.HttpContext != null;
    }

}
