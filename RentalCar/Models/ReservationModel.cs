namespace RentalCar.Models
{
	public class ReservationModel
	{

		public int UserID { get; set; } // KullanıcıID (Yabancı Anahtar)

		public int CarID { get; set; } // AraçID (Yabancı Anahtar)

		[Required(ErrorMessage = "Teslim alma tarihi seçmelisiniz.")]
		public DateTime PickupDate { get; set; } // Teslim Alış Tarihi

		[Required(ErrorMessage = "Teslim etme tarihi seçmelisiniz.")]
		public DateTime ReturnDate { get; set; } // Geri Teslim Tarihi

		[Required]
		public int TotalPrice { get; set; } // Toplam Ücret

		[Required]
		public bool Status { get; set; } = true;
	}
}
