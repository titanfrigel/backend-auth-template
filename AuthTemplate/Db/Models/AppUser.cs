using Microsoft.AspNetCore.Identity;

namespace AuthTemplate.Db.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
