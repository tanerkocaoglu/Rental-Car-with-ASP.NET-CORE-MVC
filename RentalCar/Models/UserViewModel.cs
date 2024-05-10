namespace RentalCar.Models
{
	public class UserViewModel
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
	}
}
