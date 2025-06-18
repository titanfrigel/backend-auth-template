using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackendAuthTemplate.Infrastructure.Identity
{
    public class CustomPasswordResetTokenProvider<TUser>(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<EmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger
    ) : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger) where TUser : class
    {
    }

    public class PasswordResetTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordResetTokenProviderOptions()
        {
            Name = "PasswordDataProtectorTokenProvider";
            TokenLifespan = TimeSpan.FromMinutes(30);
        }
    }
}
