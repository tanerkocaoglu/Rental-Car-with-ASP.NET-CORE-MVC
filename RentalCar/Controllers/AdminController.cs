namespace RentalCar.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController(DatabaseContext context) : Controller
	{
		public readonly DatabaseContext _context = context;

		public IActionResult Index()
		{
			ViewData["ActiveCars"] = _context.Cars.Where(x => x.Availability == true).Count();
			ViewData["UserCount"] = _context.Users.Where(x => x.Role != "admin").Count();
			ViewData["MessageCount"] = _context.Contacts.Count();
			ViewData["ActiveReservationCount"] = _context.Reservations.Where(s => s.Status == true).Count();
			return View();
		}

		public IActionResult ListContact()
		{
			var contacts = _context.
				Contacts.
				ToList();
			return View(contacts);
		}

		public IActionResult ListUser()
		{
			var user = _context.
				Users.
				Where(x => x.Role != "admin").
				ToList();
			if (user is null)
			{
				return RedirectToAction("Index", "Admin");
			}
			return View(user);
		}

		public IActionResult ActiveReservation()
		{
			var reservations = _context.Reservations.
				Where(s => s.Status == true).
				Include(c => c.Car).
				Include(u => u.User).
				ToList();
			return View(reservations);
		}

		public IActionResult ReceiveReservation(int id)
		{

			// Rezervasyon ID'sine göre rezervasyonu bul
			var reservation = _context.Reservations
				.Include(r => r.Car) // Car özelliğini dahil et
				.FirstOrDefault(r => r.ReservationID == id);

			if (reservation == null)
			{
				return RedirectToAction("ActiveReservation", "Admin"); // Rezervasyon bulunamazsa sayfayı geri döndür
			}

			// Rezervasyonun status'unu false yap
			reservation.Status = false;

			// Car özelliğinin null olmadığını kontrol et
			if (reservation.Car != null)
			{
				// Arabanın availability özelliğini true yap
				reservation.Car.Availability = true;
			}

			// Değişiklikleri veritabanına kaydet
			int affectedrows = _context.SaveChanges();

			//Veritabanı işlemi gerçekleşmezse hata döndür.
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "İşlem yapılamadı.");
			}
			// Başarılı işlem durumunda sayfayı geri döndür
			return RedirectToAction("ActiveReservation", "Admin");

		}

		public IActionResult AllReservations()
		{
			var allreservations = _context.Reservations.
				Include(c => c.Car).
				Include(u => u.User).
				ToList();
			return View(allreservations);
		}
	}
}
