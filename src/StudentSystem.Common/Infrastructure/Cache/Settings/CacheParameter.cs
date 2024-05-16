namespace StudentSystem.Common.Infrastructure.Cache.Settings
{
    public class CacheParameter
    {
        public CacheParameter(string name, object value)
        {
            Name = name;
            Value = value.ToString();
        }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
            => $"{Name}:{Value}";
    }
}
