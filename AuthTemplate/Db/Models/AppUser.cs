using Microsoft.AspNetCore.Identity;

namespace AuthTemplate.Db.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastVerificationEmailSent { get; set; }
        public DateTime? LastPasswordResetEmailSent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
