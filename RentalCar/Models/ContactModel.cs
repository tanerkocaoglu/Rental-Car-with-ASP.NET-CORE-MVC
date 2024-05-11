namespace RentalCar.Models
{
	public class ContactModel
	{
		[Required(ErrorMessage = "İsim alanını doldurmak zorunludur.")]
		[MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
		public string FullName { get; set; } // İsim

		[Required(ErrorMessage = "E-Posta alanını doldurmak zorunludur.")]
		[MaxLength(50, ErrorMessage = "Bu alan en fazla 50 karakter olabilir.")]
		public string Email { get; set; } // E-Posta

		[Required(ErrorMessage = "Telefon numarası girmek zorunludur.")]
		[MaxLength(11, ErrorMessage = "Telefon numarası 11 haneli olmalıdır.")]
		[MinLength(11, ErrorMessage = "Telefon numarası 11 haneli olmalıdır.")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Mesaj alanını doldurmak zorunludur.")]
		[MaxLength(100, ErrorMessage = "100 karakterden fazla mesaj yazılamaz.")]
		public string Message { get; set; }
	}
}
