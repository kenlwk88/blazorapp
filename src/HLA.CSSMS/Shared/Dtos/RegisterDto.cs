using System.ComponentModel.DataAnnotations;

namespace HLA.CSSMS.Shared.Dtos
{
	public class RegisterDto
	{
		[Required]
		[StringLength(128, ErrorMessage = "FirstName is required")]
		[Display(Name = "FirstName")]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[StringLength(128, ErrorMessage = "SurName is required")]
		[Display(Name = "Surname")]
		public string LastName { get; set; } = string.Empty;

		public string DisplayName { get; set; } = string.Empty;


		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; } = string.Empty;

		[Required]
        [DataType(DataType.Password)]
		[Display(Name = "Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must have minimum 8 characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; } = string.Empty;

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
