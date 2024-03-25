namespace StudentSystem.Common.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CustomizeEnumAttribute : Attribute
    {
        public CustomizeEnumAttribute(string description) 
            => this.Description = description;

        public CustomizeEnumAttribute(string description, string value)
            : this(description) => this.Value = value;

        public string Description { get; }

        public string Value { get; }
    }
}
