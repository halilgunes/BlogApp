using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models
{
	public class LoginModel
	{
		public string UserName { get; set; }

		[Required(ErrorMessage ="Lütfen bir email adresi giriniz")]
		public string Email { get; set; }
		[Required(ErrorMessage ="Lütfen şifre girişi yapınız")]
		public string Password { get; set; }
	}
}
