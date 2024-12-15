using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManufacturingManager.Core.Repositories;

namespace ManufacturingManager.Core
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(50)]
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
        public string FirstName { get; set; }

        [StringLength(50)]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        [StringLength(50)]
        [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(150)]
        [RegularExpression(RegExValidation.RegExValidEmail, ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        [DisplayName("UserRole")]
        [ForeignKey("UserRole")]
        [Required(ErrorMessage = "User Role is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select user's Role")]
        public int UserRoleId { get; set; }

        [DisplayName("Last Access")]
        public DateTime? LastAccess { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public bool IsActive { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        public string UserRoleDescription
        {
            get
            {
                if (UserRoleId != 0)
                {
                    UserRoleRepository pr = new UserRoleRepository();

                    UserRole p = pr.UserRole(UserRoleId);
                    return p.UserRoleName;
                }
                return null;
            }
        }
     
        public string FullName => FirstName + " " + LastName;
        public bool IsAdministrator => UserRoleId == 1;
        public bool IsUser => UserRoleId == 2;
        public bool IsViewer => UserRoleId == 3;
        public string Role { get; set; } //Define role for user when login
    }
}
