using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "Please input your name")]
		public string PersonName { get; set; }

		[Required(ErrorMessage = "Please input your email")]
		[EmailAddress(ErrorMessage = "Email should be a proper Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[Required(ErrorMessage = "Please input your PhoneNumber")]
		[StringLength(11, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 11)]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "Please input your password")]
		[DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long")]
        public string Password { get; set; }

		[Required(ErrorMessage = "Please confirm your password")]
		[DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }
	}
}
