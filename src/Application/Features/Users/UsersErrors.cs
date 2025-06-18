using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.Users
{
    public class UsersErrors : EntityError<UsersErrors>
    {
        protected override string Entity => "User";

        public static Error InvalidCredentials()
        {
            return new Error(
                code: $"{Instance.Entity}.InvalidCredentials",
                message: "Invalid email or password.",
                errorType: ErrorType.AccessUnAuthorized
            );
        }

        public static Error EmailNotConfirmed()
        {
            return new Error(
                code: $"{Instance.Entity}.EmailNotConfirmed",
                message: "Email not confirmed.",
                errorType: ErrorType.AccessForbidden
            );
        }

        public static Error AlreadyConfirmed()
        {
            return new Error(
                code: $"{Instance.Entity}.AlreadyConfirmed",
                message: "Email already confirmed.",
                errorType: ErrorType.Conflict
            );
        }

        public static Error InvalidToken()
        {
            return new Error(
                code: $"{Instance.Entity}.InvalidToken",
                message: "Invalid token.",
                errorType: ErrorType.Validation
            );
        }

        public static Error EmailResendTooSoon()
        {
            return new Error(
                code: $"{Instance.Entity}.EmailResendTooSoon",
                message: "Verification email recently sent.",
                errorType: ErrorType.Conflict
            );
        }

        public static Error PasswordResetTooSoon()
        {
            return new Error(
                code: $"{Instance.Entity}.PasswordResetTooSoon",
                message: "Password reset recently requested.",
                errorType: ErrorType.Conflict
            );
        }
    }
}
