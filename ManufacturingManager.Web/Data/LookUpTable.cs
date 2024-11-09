using System.ComponentModel.DataAnnotations;
using ManufacturingManager.Core;

namespace ManufacturingManager.Web.Data
{
    public class LookUpTable
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
        public string Description { get; set; } = null!;

        public bool Active { get; set; }

        public int TimesUsed { get; set; }
    }
}
