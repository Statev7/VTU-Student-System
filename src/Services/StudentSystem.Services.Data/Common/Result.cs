namespace StudentSystem.Services.Data.Common
{
    public class Result
    {
        public bool Succeed { get; private set; }

        public string Error { get; private set; } = null!;

        public static implicit operator Result(bool succeed)
            => new Result() { Succeed = succeed };

        public static implicit operator Result(string error)
            => new Result()
            {
                Succeed = false,
                Error = error
            };
    }
}
