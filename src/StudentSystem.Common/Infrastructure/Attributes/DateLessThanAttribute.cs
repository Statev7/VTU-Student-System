namespace StudentSystem.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class DateLessThanAttribute : ValidationAttribute
    {
        private const string PropertyNotExistErrorMessage = "{0} property don't exist";
        private const string InvalidDateRangeErrorMessage = "The {0} cannot be before {1}";

        private readonly string comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty) 
            => this.comparisonProperty = comparisonProperty;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var secondDateAsPropertyInfo = 
                context.ObjectType.GetProperty(this.comparisonProperty) 
                ?? throw new ArgumentNullException(string.Format(PropertyNotExistErrorMessage, comparisonProperty));

            var firstDateValue = (DateTime)value;
            var secondDateValue = (DateTime)secondDateAsPropertyInfo.GetValue(context.ObjectInstance);

            if (firstDateValue > secondDateValue)
            {
                return new ValidationResult(this.CreateErrorMessage(secondDateAsPropertyInfo, context));
            }

            return ValidationResult.Success;
        }

        private string CreateErrorMessage(PropertyInfo secondDate, ValidationContext context)
        {
            var secondDateDisplayAttribute = secondDate
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault();

            var secondDateName = secondDateDisplayAttribute == null
                    ? secondDate.Name
                    : secondDateDisplayAttribute.Name;

            var errorMessage = string.Format(InvalidDateRangeErrorMessage, secondDateName, context.DisplayName);

            return errorMessage;
        }
    }
}
