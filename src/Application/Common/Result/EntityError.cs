namespace BackendAuthTemplate.Application.Common.Result
{
    public abstract class EntityError<T> where T : EntityError<T>, new()
    {
        protected abstract string Entity { get; }

        private static readonly T Instance = new();

        public static Error Failure()
        {
            return new Error(
                code: $"{Instance.Entity}.{ErrorType.Failure}",
                message: $"{Instance.Entity} operation failed.",
                errorType: ErrorType.Failure
            );
        }

        public static Error NotFound(Guid id)
        {
            return new Error(
                code: $"{Instance.Entity}.{id}.{ErrorType.NotFound}",
                message: $"{Instance.Entity} with ID '{id}' was not found.",
                errorType: ErrorType.NotFound
            );
        }

        public static Error Validation(string field)
        {
            return new Error(
                code: $"{Instance.Entity}.{field}.{ErrorType.Validation}",
                message: $"Validation error on field '{field}'.",
                errorType: ErrorType.Validation
            );
        }

        public static Error Conflict(string field)
        {
            return new Error(
                code: $"{Instance.Entity}.{field}.{ErrorType.Conflict}",
                message: $"Conflict detected for field '{field}'.",
                errorType: ErrorType.Conflict
            );
        }

        public static Error AccessUnAuthorized()
        {
            return new Error(
                code: $"{Instance.Entity}.{ErrorType.AccessUnAuthorized}",
                message: $"Unauthorized access to {Instance.Entity}.",
                errorType: ErrorType.AccessUnAuthorized
            );
        }

        public static Error AccessForbidden()
        {
            return new Error(
                code: $"{Instance.Entity}.{ErrorType.AccessForbidden}",
                message: $"Access to {Instance.Entity} is forbidden.",
                errorType: ErrorType.AccessForbidden
            );
        }
    }
}
