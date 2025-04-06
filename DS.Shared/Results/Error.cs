namespace DS.Shared.Results
{
    public class Error : IEquatable<Error>
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NotFound = new("NotFound", "The requested item was not found.");
        public static Error Validation(string details) => new("Validation", details);

        public bool Equals(Error? other) =>
            other is not null && Code == other.Code && Message == other.Message;

        public override bool Equals(object? obj) => obj is Error error && Equals(error);
        public override int GetHashCode() => HashCode.Combine(Code, Message);
        public override string ToString() => Code;
    }
}
