namespace RentalCar.Models
{
	public class CarModel
	{
		public int CarID { get; set; }

		[Required(ErrorMessage = "Marka alanını doldurmak zorunludur.")]
		[MinLength(3, ErrorMessage = "En az 3 karakter girmeilisiniz.")]
		[MaxLength(50, ErrorMessage = "En fazla 50 karakter girebilirsiniz.")]
		public string Brand { get; set; } // Marka

		[Required(ErrorMessage = "Model alanını doldurmak zorunludur.")]
		[MinLength(3, ErrorMessage = "En az 3 karakter girmeilisiniz.")]
		[MaxLength(50, ErrorMessage = "En fazla 50 karakter girebilirsiniz.")]
		public string Model { get; set; } // Model

		[Required(ErrorMessage = "Yıl alanını doldurmak zorunludur.")]
		public int Year { get; set; } // Yıl

		[Required(ErrorMessage = "Yakıt türü alanını doldurmak zorunludur.")]
		public string FuelType { get; set; } // Yakıt Türü

		[Required(ErrorMessage = "Vites tipi alanını doldurmak zorunludur.")]
		public string TransmissionType { get; set; } // Vites Tipi

		[Required(ErrorMessage = "Günlük ücret alanını doldurmak zorunludur.")]
		public int DailyRate { get; set; } // Günlük Ücret

		[Required]
		public bool Availability { get; set; } = true; // Durum

		[Required(ErrorMessage = "Resim seçiniz.")]
		public  IFormFile ImageFile { get; set; }
	}
	
}
