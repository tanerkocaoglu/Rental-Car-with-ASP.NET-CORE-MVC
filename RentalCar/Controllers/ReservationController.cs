namespace RentalCar.Controllers
{
	public class ReservationController(DatabaseContext context) : Controller
	{
		public readonly DatabaseContext _context = context;

		public IActionResult ActiveReservations()
		{
			if (User.Identity.IsAuthenticated)
			{
				var reservastions = _context.Reservations.Where(x => x.Status == true && x.UserID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).Include(u => u.Car).ToList();
				return View(reservastions);
			}
			return View();
		}

		public IActionResult TakeReservation(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				var selectedCar = _context.Cars.FirstOrDefault(x => x.CarID == id);
				if (selectedCar is null)
				{
					return RedirectToAction("Cars", "Home");
				}
				TempData["SelectedCar"] = selectedCar;
				return View();
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}

		[HttpPost]
		public IActionResult TakeReservation(ReservationViewModel reservationModel)
		{
			if (ModelState.IsValid)
			{
				var selectedCar = _context.Cars.FirstOrDefault(x => x.CarID == reservationModel.CarID);
				if (selectedCar == null)
				{
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
					ModelState.AddModelError("", "Kayıt yapılamadı.");
				}

				return RedirectToAction("ActiveReservations", "Reservation");
			}

			return RedirectToAction("Cars", "Home");

		}

	}
}
