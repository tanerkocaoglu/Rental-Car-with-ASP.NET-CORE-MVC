using System.Drawing;

namespace RentalCar.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Ad alanını doldurmak zorunludur.")]
		[StringLength(50)]
		public string FullName { get; set; } // Ad


		[Required(ErrorMessage = "Kullanıcı adı alanını doldurmak zorunludur.")]
		[StringLength(50)]
		public string Username { get; set; } // Kullanıcı Adı

		[Required(ErrorMessage = "E-Posta alanını doldurmak zorunludur.")]
		[StringLength(50)]
		public string Email { get; set; } // E-Posta

		[Required(ErrorMessage = "Şifre alanını doldurmak zorunludur.")]
		[MinLength(6, ErrorMessage = "Şifre 6 karakterden az olamaz.")]
		[MaxLength(16, ErrorMessage = "Şifre 16 karakterden fazla olamaz.")]
		public string Password { get; set; } // Şifre

		[Required(ErrorMessage = "Şifreyi tekrar giriniz.")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
		[MaxLength(16, ErrorMessage = "Şifre en fazla 16 karakter olabilir.")]
		[Compare(nameof(Password), ErrorMessage = "Girdiğiniz şifreler uyuşmuyor.")]
		public string RePassword { get; set; }

	}
}
