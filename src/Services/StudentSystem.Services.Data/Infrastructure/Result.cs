namespace StudentSystem.Services.Data.Infrastructure
{
    public class Result
    {
        internal Result(bool succeed, string message)
        {
            this.Succeed = succeed;
            this.Message = message;
        }

        public bool Succeed { get; private set; }

        public string Message { get; private set; } = null!;

        public static implicit operator Result(bool succeed)
            => new Result(succeed, string.Empty);

        public static implicit operator Result(string error)
            => new Result(false, error);

        public static Result Success(string message)
            => new Result(true, message);
    }

    public class Result<TData> : Result
    {
        private readonly TData data;

        private Result(bool succeeded, TData data, string message)
            : base(succeeded, message)
            => this.data = data;

        public TData Data
            => this.Succeed
                ? this.data
                : throw new InvalidOperationException($"{nameof(this.Data)} is not available with a failed result. Use {this.Message} instead.");

        public static Result<TData> SuccessWith(TData data, string message = null)
            => new Result<TData>(true, data, message);

        public new static Result<TData> Failure(string message)
            => new Result<TData>(false, default!, message);
    }
}