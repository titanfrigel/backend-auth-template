namespace BackendAuthTemplate.Application.Common.Result
{
    public class Error(
        string code,
        string message,
        ErrorType errorType,
        IReadOnlyDictionary<string, string[]>? details = null
    )
    {
        public readonly string Code = code;
        public readonly string Message = message;
        public readonly ErrorType ErrorType = errorType;
        public readonly IReadOnlyDictionary<string, string[]>? Details = details;
    }
}
