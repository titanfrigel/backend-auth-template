namespace AuthTemplate.Dtos
{
    public class AuthRegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ResendVerificationDto
    {
        public string Email { get; set; } = null!;
    }

    public class ConfirmEmailDto
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; } = null!;
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class AuthLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = true;
    }

    public class AuthRefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class AuthTokenReadDto
    {
        public string AccessToken { get; set; } = null!;
    }
}
