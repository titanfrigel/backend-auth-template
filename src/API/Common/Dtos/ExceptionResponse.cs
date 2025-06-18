namespace BackendAuthTemplate.API.Common.Dtos
{
    public class ExceptionResponse
    {
        public string Code { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? StackTrace { get; set; } = null;
    }
}
