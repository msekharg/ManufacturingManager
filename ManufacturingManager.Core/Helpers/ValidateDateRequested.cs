/* Summary
 This will be used for future enhancement
 */

using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core.Helpers
{
    public class ValidateDateRequested : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var today = DateTime.Now;
            if (DateTime.TryParse(value?.ToString(), out var requestedDate))
            {
                return today > requestedDate;

            }

            return false;
        }
    }
}
