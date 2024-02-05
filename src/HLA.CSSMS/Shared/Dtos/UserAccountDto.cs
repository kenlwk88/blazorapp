using System.ComponentModel.DataAnnotations;

namespace HLA.CSSMS.Shared.Dtos
{
    public class UserAccountDto
    {
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Username must be larger than 7 charactors!")]
        [Display(Name = "UserName")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "A valid is required")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128, ErrorMessage = "FirstName is required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(128, ErrorMessage = "Surname is required")]
        [Display(Name = "Surname")]
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must have minimum 8 characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string UserRole { get; set; } = "User";
        public bool IsSuperUser { get; set; } = false;
    }
}
