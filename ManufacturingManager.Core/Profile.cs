using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "Role's description is required")]
        [Display(Name = "Role's Description")]
        [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
        public string UserRoleName { get; set; }

        public bool IsActive { get; set; }

    }
}
