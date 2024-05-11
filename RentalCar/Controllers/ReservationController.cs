namespace RentalCar.Controllers
{
	public class ReservationController(DatabaseContext context) : Controller
	{
		public readonly DatabaseContext _context = context;

		//Aktif rezervasyonlar sayfası
		public IActionResult ActiveReservations()
		{
			// Kullanıcı kimliği doğrulanmış mı diye kontrol et
			if (User.Identity.IsAuthenticated)
			{
				// Kullanıcının aktif rezervasyonlarını getir
				var reservastions = _context.Reservations.Where(x => x.Status == true && x.UserID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).Include(u => u.Car).ToList();
				return View(reservastions);
			}
			// Kimlik doğrulama yapılmamışsa boş bir görünüm döndür
			return View();
		}


		//Rezervsayon alma işlemeri
		#region
		public IActionResult TakeReservation(int id)
		{
			// Kullanıcı kimliği doğrulanmış mı diye kontrol et
			if (User.Identity.IsAuthenticated)
			{
				// Seçilen aracı bul
				var selectedCar = _context.Cars.FirstOrDefault(x => x.CarID == id);
				if (selectedCar is null)
				{
					// Eğer araç bulunamazsa Ana Sayfa'ya yönlendir
					return RedirectToAction("Cars", "Home");
				}
				// Seçilen aracı geçici verilere ekle
				TempData["SelectedCar"] = selectedCar;
				return View();
			}
			else
			{
				// Kullanıcı kimliği doğrulanmamışsa Giriş yap sayfasına yönlendir
				return RedirectToAction("Login", "Account");
			}
		}

		[HttpPost]
		public IActionResult TakeReservation(ReservationModel reservationModel)
		{
			// Model doğrulamasını kontrol et
			if (ModelState.IsValid)
			{
				// Seçilen aracı bul
				var selectedCar = _context.Cars.FirstOrDefault(x => x.CarID == reservationModel.CarID);
				if (selectedCar == null)
				{
					// Eğer araç bulunamazsa Ana Sayfa'ya yönlendir
					return RedirectToAction("Cars", "Home");
				}

				// Yeni rezervasyon oluştur
				Reservation reservation = new()
				{
					UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
					CarID = reservationModel.CarID,
					PickupDate = reservationModel.PickupDate,
					ReturnDate = reservationModel.ReturnDate,
					TotalPrice = reservationModel.TotalPrice,
				};
				selectedCar.Availability = false; // Arabanın Availability özelliğini false olarak güncelle

				_context.Reservations.Add(reservation); // Yeni rezervasyonu veritabanına ekle
				int affectedrows = _context.SaveChanges(); // Değişiklikleri kaydet

				if (affectedrows == 0)
				{
					// Eğer kayıt yapılamazsa hata ekle
					ModelState.AddModelError("", "Kayıt yapılamadı.");
				}

				// Aktif rezervasyonlara yönlendir
				return RedirectToAction("ActiveReservations", "Reservation");
			}

			// Eğer model doğrulaması başarısız olursa Ana Sayfa'ya yönlendir
			return RedirectToAction("Cars", "Home");
		}

		#endregion
	}
}
