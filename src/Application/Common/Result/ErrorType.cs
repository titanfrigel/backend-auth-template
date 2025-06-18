namespace BackendAuthTemplate.Application.Common.Result
{
    public enum ErrorType
    {
        Failure = 0,
        NotFound = 1,
        Validation = 2,
        Conflict = 3,
        AccessUnAuthorized = 4,
        AccessForbidden = 5
    }
}
