namespace BackendAuthTemplate.Application.Common.Result
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        private Result(TValue value) : base()
        {
            _value = value;
        }

        private Result(Error error) : base(error)
        {
            _value = default;
        }

        public TValue Value => Succeeded ? _value! : throw new InvalidOperationException("Value can not be accessed when Succeeded is false");

        public static Result<TValue> Success(TValue value)
        {
            return new(value);
        }

        public static new Result<TValue> Failure(Error error)
        {
            return new(error);
        }

        public static implicit operator Result<TValue>(Error error)
        {
            return new(error);
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            return new(value);
        }
    }

    public class Result
    {
        public readonly bool Succeeded;
        public readonly Error? Error;

        protected Result()
        {
            Succeeded = true;
            Error = default;
        }

        protected Result(Error? error = default)
        {
            Succeeded = false;
            Error = error;
        }

        public static Result Success()
        {
            return new();
        }

        public static Result Failure(Error error)
        {
            return new(error);
        }

        public static implicit operator Result(Error error)
        {
            return new(error);
        }
    }
}
