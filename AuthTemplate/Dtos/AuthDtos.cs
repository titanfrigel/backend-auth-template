namespace AuthTemplate.Dtos
{
    public class AuthRegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthRefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class AuthTokenReadDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
