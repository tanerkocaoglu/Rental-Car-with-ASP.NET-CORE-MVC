using System.Diagnostics;

namespace RentalCar.Controllers
{
	public class HomeController(DatabaseContext context) : Controller
	{

		private readonly DatabaseContext _context = context;

		// Ana sayfa iþlemi
		public IActionResult Index()
		{
			return View(); // Ana sayfa sayfasýný view'e yönlendir
		}

		// Hakkýnda sayfasý iþlemi
		public IActionResult About()
		{
			return View(); // Hakkýnda sayfasýný view'e yönlendir
		}

		//Ýletiþim Sayfasý
		#region 

		[HttpGet]
		// Ýletiþim formu get iþlemi
		public IActionResult Contact()
		{
			return View(); // Ýletiþim formununu view'e yönlendir
		}

		[HttpPost]
		// Ýletiþim formu post iþlemi
		public IActionResult Contact(ContactModel contactmodel)
		{
			if (ModelState.IsValid) // Eðer model geçerliyse
			{
				// Yeni iletiþim nesnesi oluþtur
				Contact contact = new()
				{
					FullName = contactmodel.FullName,
					Email = contactmodel.Email,
					PhoneNumber = contactmodel.PhoneNumber,
					Message = contactmodel.Message
				};

				_context.Contacts.Add(contact); // Ýletiþim nesnesini veritabanýna ekle
				int affectedrows = _context.SaveChanges(); // Deðiþiklikleri kaydet
				if (affectedrows == 0)
				{
					ModelState.AddModelError("", "Mesaj gönderilemedi"); // Eðer gönderme baþarýsýz olursa hata mesajý ekle
				}
				ViewData["isSuccess"] = "Mesajýnýz baþarýyla iletilmiþtir."; // Baþarýlý gönderim mesajý
				return RedirectToAction("Contact", "Home"); // Ýletiþim sayfasýna yönlendir
			}
			return View(); // Geçersiz model durumunda iletiþim formunu tekrar view'e yönlendir
		}

		#endregion

		//Arabalar Sayfasý
		#region

		// Araçlarý listeleme iþlemi
		public IActionResult Cars()
		{
			// Araçlardan müsaitlik durumu true olanlar günlük kiralama ücretine göre azalan þekilde sýralanýyor
			var cars = _context.Cars.ToList().Where(x => x.Availability == true).OrderByDescending(x => x.DailyRate);

			// ViewData ile müsaitlik durumu true olan araçlarýn sayýsý aktarýlýyor
			ViewData["CarCount"] = _context.Cars.Where(x => x.Availability == true).Count();

			return View(cars); // Araba view'e müsaitlik durumu true olan arabalarý göndererek yönlendirme yapýlýyor
		}

		#endregion

		//Error
		#region
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		#endregion
	}
}
