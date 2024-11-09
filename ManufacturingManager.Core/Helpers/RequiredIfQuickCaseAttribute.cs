using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core.Helpers
{
    internal class RequiredIfQuickCaseAttribute : ValidationAttribute
    {
        public string _dependent;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectInstance.GetType().GetProperty(_dependent);
            var propertyValue = property?.GetValue(validationContext.ObjectInstance, null);
            var isValidationEnabled = propertyValue != null && (bool)propertyValue;
            if (isValidationEnabled && value == null)
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
