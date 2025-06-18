namespace BackendAuthTemplate.API.Common.Dtos
{
    public class ErrorResponse
    {
        public string Code { get; set; } = default!;
        public string Message { get; set; } = default!;
        public IReadOnlyDictionary<string, string[]>? Details { get; set; }
    }
}
