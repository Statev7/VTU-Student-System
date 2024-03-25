namespace StudentSystem.Common.Infrastructure.Extensions
{
    using System.Reflection;

    using StudentSystem.Common.Infrastructure.Attributes;

    public static class EnumExtensions
    {
        public static string GetEnumValue(this Enum value)
            => GetCustomizeEnumAttribute(value).Value;

        public static string GetEnumDescription(this Enum value)
            => GetCustomizeEnumAttribute(value).Description;

        private static CustomizeEnumAttribute GetCustomizeEnumAttribute(Enum value) 
            => value
                ?.GetType()
                ?.GetField(value.ToString())
                ?.GetCustomAttribute(typeof(CustomizeEnumAttribute)) as CustomizeEnumAttribute
                ?? new CustomizeEnumAttribute(string.Empty, string.Empty);
    }
}
