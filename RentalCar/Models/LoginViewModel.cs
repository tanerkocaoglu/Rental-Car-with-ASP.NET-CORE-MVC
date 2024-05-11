namespace RentalCar.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Kullanıcı adı alanını doldurmak zorunludur.")]
		[MaxLength(50, ErrorMessage = "Kullanıcı adı 50 karakterden fazla olamaz.")]
		public string Username { get; set; } // Kullanıcı Adı

		[Required(ErrorMessage = "Şifre alanını doldurmak zorunludur.")]
		[MinLength(6, ErrorMessage = "Şifre 6 karakterden az olamaz.")]
		[MaxLength(16, ErrorMessage = "Şifre 16 karakterden fazla olamaz.")]
		public string Password { get; set; } // Şifre

	}
}
