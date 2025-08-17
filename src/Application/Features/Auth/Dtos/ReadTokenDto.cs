namespace BackendAuthTemplate.Application.Features.Auth.Dtos
{
    public class ReadTokenDto
    {
        public required string AccessToken { get; init; }
        public string? RefreshToken { get; init; } = null;
    }
}
