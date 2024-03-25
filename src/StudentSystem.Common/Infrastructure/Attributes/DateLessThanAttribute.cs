namespace StudentSystem.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateLessThanAttribute : ValidationAttribute
    {
        private const string PropertyNotExistErrorMessage = "{0} property don't exist";
        private const string InvalidDateRangeErrorMessage = "The {0} cannot be before {1}";
        private const string InvalidPropertyTypeErrorMessage = $"{nameof(DateLessThanAttribute)} can be applied only on properties of DateTime type";

        public DateLessThanAttribute(string comparisonDate) 
            => this.ComparisonDate = comparisonDate;

        public string ComparisonDate { get; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var comparisonDateMetaData = 
                context.ObjectType.GetProperty(this.ComparisonDate) 
                ?? throw new ArgumentNullException(string.Format(PropertyNotExistErrorMessage, ComparisonDate));

            var comparisonDate = comparisonDateMetaData.GetValue(context.ObjectInstance);

            if (value is not DateTime || comparisonDate is not DateTime)
            {
                throw new ArgumentException(InvalidPropertyTypeErrorMessage);
            }

            var firstDateValue = (DateTime)value;
            var comparisonDateValue = (DateTime)comparisonDate;

            if (firstDateValue > comparisonDateValue)
            {
                return new ValidationResult(this.GenerateErrorMessage(comparisonDateMetaData, context));
            }

            return ValidationResult.Success;
        }

        private string GenerateErrorMessage(PropertyInfo comparisonDate, ValidationContext context)
        {
            var comparisonDateDisplayAttribute = comparisonDate
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault();

            var comparisonDateName = comparisonDateDisplayAttribute == null
                    ? comparisonDate.Name
                    : comparisonDateDisplayAttribute.Name;

            var errorMessage = string.Format(InvalidDateRangeErrorMessage, comparisonDateName, context.DisplayName);

            return errorMessage;
        }
    }
}
