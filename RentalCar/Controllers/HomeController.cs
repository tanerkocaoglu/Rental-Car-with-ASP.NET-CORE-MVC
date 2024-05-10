using System.Diagnostics;

namespace RentalCar.Controllers
{
	public class HomeController(DatabaseContext context) : Controller
	{
	
		private readonly DatabaseContext _context = context;

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		//�leti�im Sayfas�
		#region 
		[HttpGet]
		public IActionResult Contact()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Contact(ContactViewModel contactmodel)
		{
			if (ModelState.IsValid)
			{
				Contact contact = new()
				{
					FullName = contactmodel.FullName,
					Email = contactmodel.Email,
					PhoneNumber = contactmodel.PhoneNumber,
					Message = contactmodel.Message
				};

				_context.Contacts.Add(contact);
				int affectedrows = _context.SaveChanges();
				if (affectedrows == 0)
				{
					ModelState.AddModelError("", "Mesaj g�nderilemedi");
				}
				ViewData["isSuccess"] = "Mesaj�n�z ba�ar�yla iletilmi�tir.";
				return RedirectToAction("Contact", "Home");
			}
			return View();
		}
		#endregion


		//Arabalar Sayfas�
		#region
		public IActionResult Cars()
		{
			var cars = _context.Cars.ToList().Where(x => x.Availability == true).OrderByDescending(x => x.DailyRate);
			ViewData["CarCount"] = _context.Cars.Where(x => x.Availability == true).Count();

			return View(cars);
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
