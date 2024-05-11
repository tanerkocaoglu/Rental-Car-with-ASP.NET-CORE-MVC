using System.Diagnostics;

namespace RentalCar.Controllers
{
	public class HomeController(DatabaseContext context) : Controller
	{

		private readonly DatabaseContext _context = context;

		// Ana sayfa i�lemi
		public IActionResult Index()
		{
			return View(); // Ana sayfa sayfas�n� view'e y�nlendir
		}

		// Hakk�nda sayfas� i�lemi
		public IActionResult About()
		{
			return View(); // Hakk�nda sayfas�n� view'e y�nlendir
		}

		//�leti�im Sayfas�
		#region 

		[HttpGet]
		// �leti�im formu get i�lemi
		public IActionResult Contact()
		{
			return View(); // �leti�im formununu view'e y�nlendir
		}

		[HttpPost]
		// �leti�im formu post i�lemi
		public IActionResult Contact(ContactModel contactmodel)
		{
			if (ModelState.IsValid) // E�er model ge�erliyse
			{
				// Yeni ileti�im nesnesi olu�tur
				Contact contact = new()
				{
					FullName = contactmodel.FullName,
					Email = contactmodel.Email,
					PhoneNumber = contactmodel.PhoneNumber,
					Message = contactmodel.Message
				};

				_context.Contacts.Add(contact); // �leti�im nesnesini veritaban�na ekle
				int affectedrows = _context.SaveChanges(); // De�i�iklikleri kaydet
				if (affectedrows == 0)
				{
					ModelState.AddModelError("", "Mesaj g�nderilemedi"); // E�er g�nderme ba�ar�s�z olursa hata mesaj� ekle
				}
				ViewData["isSuccess"] = "Mesaj�n�z ba�ar�yla iletilmi�tir."; // Ba�ar�l� g�nderim mesaj�
				return RedirectToAction("Contact", "Home"); // �leti�im sayfas�na y�nlendir
			}
			return View(); // Ge�ersiz model durumunda ileti�im formunu tekrar view'e y�nlendir
		}

		#endregion

		//Arabalar Sayfas�
		#region

		// Ara�lar� listeleme i�lemi
		public IActionResult Cars()
		{
			// Ara�lardan m�saitlik durumu true olanlar g�nl�k kiralama �cretine g�re azalan �ekilde s�ralan�yor
			var cars = _context.Cars.ToList().Where(x => x.Availability == true).OrderByDescending(x => x.DailyRate);

			// ViewData ile m�saitlik durumu true olan ara�lar�n say�s� aktar�l�yor
			ViewData["CarCount"] = _context.Cars.Where(x => x.Availability == true).Count();

			return View(cars); // Araba view'e m�saitlik durumu true olan arabalar� g�ndererek y�nlendirme yap�l�yor
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
