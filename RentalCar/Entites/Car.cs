namespace RentalCar.Entites
{
	public class Car
	{

		[Key]
		public int CarID { get; set; } // AraçID (Anahtar)

		[Required, StringLength(50)]
		public string Brand { get; set; } // Marka

		[Required, StringLength(50)]
		public string Model { get; set; } // Model

		[Required]
		public int Year { get; set; } // Yıl

		[Required, StringLength(20)]
		public string FuelType { get; set; } // Yakıt Türü

		[Required, StringLength(20)]
		public string TransmissionType { get; set; } // Vites Tipi

		[Required]
		public int DailyRate { get; set; } // Günlük Ücret

		[Required]
		public bool Availability { get; set; } = true; // Durum

		[Required]
		public string ImageFile { get; set; }
	}
}
