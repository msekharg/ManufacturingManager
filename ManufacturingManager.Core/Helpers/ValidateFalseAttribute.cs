using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core.Helpers;

public class ValidateFalseAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is bool booleanValue && booleanValue == true)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("The field must be true.");
    }
}