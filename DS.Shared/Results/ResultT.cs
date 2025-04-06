namespace DS.Shared.Results
{
    public class Result<T> : Result
    {
        public T Value { get; }

        protected internal Result(T value, bool isSuccess, Error error)
            : base(isSuccess, error) => Value = value;

        public static Result<T> Success(T value) => new(value, true, Error.None);
        public new static Result<T> Failure(Error error) => new(default!, false, error);
    }
}
