namespace StudentSystem.Common.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private const string NotAllowedFileExtensionErrorMessage = "{0} extension is not allowed! The allowed types are: {1}";

        private readonly string extensions;

        public AllowedExtensionsAttribute(string extensions) 
            => this.extensions = extensions;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                var extensionAsArray = this.extensions.Split(' ');

                if (!extensionAsArray.Contains(extension.ToLower()))
                {
                    var errorMessage = string.Format(NotAllowedFileExtensionErrorMessage, extension, string.Join(" ", extensionAsArray));

                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
