namespace RentalCar.Entites
{
	public class Contact
	{
		[Key]
		public int MessageId { get; set; } // MesajID(Anahtar)

		[Required,StringLength(50)]
		public string FullName { get; set; } // İsim

		[Required, StringLength(50)]
		public string Email { get; set; } // E-Posta

		[Required, StringLength(11)]
		public string PhoneNumber { get; set; } // Telefon Numarası

		[Required, StringLength(100)]
		public string Message { get; set; } // İleti

	}
}
