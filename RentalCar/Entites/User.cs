namespace RentalCar.Entites
{
	public class User
	{
		[Key]
		public int UserId { get; set; } // KullanıcıID(Anahtar)

		[Required, StringLength(50)]
		public string FullName { get; set; } // Ad

		[Required, StringLength(50)]
		public string Username { get; set; } // Kullanıcı Adı

		[Required, StringLength(100)]
		public string Password { get; set; } // Şifre

		[Required, StringLength(50)]
		public string Email { get; set; } // E-Posta

		[Required, StringLength(10)]
		public string Role { get; set; } = "user"; // Kullanıcı Rolü
	}
}
