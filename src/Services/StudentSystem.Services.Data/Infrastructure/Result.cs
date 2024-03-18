namespace StudentSystem.Services.Data.Infrastructure
{
    public class Result
    {
        private Result(bool succeed, string message)
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
}
