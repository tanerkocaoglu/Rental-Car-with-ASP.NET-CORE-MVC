namespace RentalCar.Entites
{
	public class Reservation
	{
		[Key]
		public int ReservationID { get; set; } // RezervasyonID (Anahtar)

		public int UserID { get; set; } // KullanıcıID (Yabancı Anahtar)
		[ForeignKey("UserID")]
		[ValidateNever]
		public User User { get; set; } // Kullanıcı tablosuyla ilişki

		public int CarID { get; set; } // AraçID (Yabancı Anahtar)
		[ForeignKey("CarID")]
		[ValidateNever]
		public Car Car { get; set; } // Araçlar tablosuyla ilişki

		[Required]
		public DateTime PickupDate { get; set; } // Teslim Alış Tarihi

		[Required]
		public DateTime ReturnDate { get; set; } // Geri Teslim Tarihi

		[Required]
		public int TotalPrice { get; set; } // Toplam Ücret

		[Required]
		public bool Status { get; set; } = true; // Rezervason Durumu
	}
}
