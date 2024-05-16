namespace RentalCar.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController(DatabaseContext context) : Controller
	{
		public readonly DatabaseContext _context = context;

		// Ana sayfa işlemi
		public IActionResult Index()
		{
			// ViewData aracılığıyla müsait araba sayısı aktarılıyor
			ViewData["ActiveCars"] = _context.Cars.Where(x => x.Availability == true).Count();

			// ViewData aracılığıyla kullanıcı sayısı aktarılıyor (admin dışındaki kullanıcılar)
			ViewData["UserCount"] = _context.Users.Where(x => x.Role != "admin").Count();

			// ViewData aracılığıyla iletişim mesajı sayısı aktarılıyor
			ViewData["MessageCount"] = _context.Contacts.Count();

			// ViewData aracılığıyla aktif rezervasyon sayısı aktarılıyor
			ViewData["ActiveReservationCount"] = _context.Reservations.Where(s => s.Status == true).Count();

			return View(); // Ana sayfa view'e yönlendiriliyor
		}


		// Mesajları listeleme
		public IActionResult ListContact()
		{
			// Tüm mesajlar veritabanından alınıyor ve bir liste olarak view'e iletiliyor
			var contacts = _context.Contacts.ToList();
			return View(contacts); // Mesajlarlar view'e yönlendiriliyor
		}


		// Kullanıcıları listeleme 
		public IActionResult ListUser()
		{
			// Rolü admin olmayan kullanıcılar alınıyor ve bir listeye aktarılıyor
			var users = _context.Users.Where(x => x.Role != "admin").ToList();

			// Eğer kullanıcı bulunamazsa, Admin kontrol paneline yönlendiriliyor
			if (users.Count == 0)
			{
				return RedirectToAction("Index", "Admin");
			}

			return View(users); // Kullanıcılar view'e yönlendiriliyor
		}


		// Aktif rezervasyonları listeleme
		public IActionResult ActiveReservation()
		{
			// Durumu true olan rezervasyonlar alınıyor, araba ve kullanıcı bilgileri ile birlikte
			var reservations = _context.Reservations
				.Where(s => s.Status == true)
				.Include(c => c.Car)
				.Include(u => u.User)
				.ToList();

			return View(reservations); // Aktif rezervasyonlar view'e yönlendiriliyor
		}


		// Rezervasyon alımı işlemi
		public IActionResult ReceiveReservation(int id)
		{
			// Belirli bir rezervasyonun detayları alınıyor, araba bilgisiyle birlikte
			var reservation = _context.Reservations
				.Include(r => r.Car)
				.FirstOrDefault(r => r.ReservationID == id);

			// Eğer rezervasyon bulunamazsa, Admin kontrol paneline yönlendiriliyor
			if (reservation == null)
			{
				return RedirectToAction("ActiveReservation", "Admin");
			}

			// Rezervasyonun durumu alındı olarak güncelleniyor
			reservation.Status = false;

			// Eğer rezervasyona ait araba bilgisi varsa, arabanın müsaitlik durumu true olarak güncelleniyor
			if (reservation.Car != null)
			{
				reservation.Car.Availability = true;
			}

			// Değişiklikler veritabanına kaydediliyor
			int affectedrows = _context.SaveChanges();

			// Eğer hiçbir satır etkilenmediyse, model durumuna hata ekleniyor
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "İşlem yapılamadı.");
			}

			return RedirectToAction("ActiveReservation", "Admin"); // Aktif rezervasyonlar viewine yönlendiriliyor
		}


		// Tüm rezervasyonları listeleme işlemi
		public IActionResult AllReservations()
		{
			// Tüm rezervasyonlar alınıyor, araba ve kullanıcı bilgileri ile birlikte
			var allReservations = _context.Reservations
				.Include(c => c.Car)
				.Include(u => u.User)
				.ToList();

			return View(allReservations); // Tüm rezervasyonlar view'e yönlendiriliyor
		}


	}
}
