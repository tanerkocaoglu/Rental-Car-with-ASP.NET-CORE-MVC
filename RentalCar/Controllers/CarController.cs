namespace RentalCar.Controllers
{
	[Authorize(Roles = "admin")]
	public class CarController(DatabaseContext context, IWebHostEnvironment environment) : Controller
	{
		public readonly DatabaseContext _context = context;
		public readonly IWebHostEnvironment _environment = environment;

		//Araba Listeleme İşlemleri
		#region
		public IActionResult Index()
		{
			var cars = _context.Cars.ToList();
			return View(cars);
		}
		#endregion


		//Araba Ekleme İşlemleri
		#region
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(CarViewModel carmodel)
		{
			if (ModelState.IsValid)
			{
				string filename = "";
				if (carmodel.ImageFile != null)
				{
					String uploadfolder = Path.Combine(_environment.WebRootPath, "images");
					filename = Guid.NewGuid().ToString() + "_" + carmodel.ImageFile.FileName;
					string filepath = Path.Combine(uploadfolder, filename);
					carmodel.ImageFile.CopyTo(new FileStream(filepath, FileMode.Create));
				}

				Car car = new()
				{
					Brand = carmodel.Brand,
					Model = carmodel.Model,
					Year = carmodel.Year,
					FuelType = carmodel.FuelType,
					TransmissionType = carmodel.TransmissionType,
					DailyRate = carmodel.DailyRate,
					Availability = carmodel.Availability,
					ImageFile = filename,
				};

				_context.Cars.Add(car);
				int affectedrows = _context.SaveChanges();
				ViewBag.success = "Kayıt başarıyla eklendi.";
				if (affectedrows == 0)
				{
					ModelState.AddModelError("", "Araba Eklenemedi.");
				}
				return RedirectToAction("Index");
			}

			return View(carmodel);
		}

		#endregion


		//Araba Silme İşlemleri
		#region

		public IActionResult Delete(int id)
		{
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				return RedirectToAction("Index", "Car");
			}

			string filepath = _environment.WebRootPath + "images" + car.ImageFile;
			System.IO.File.Delete(filepath);

			_context.Cars.Remove(car);
			int affectedrows = _context.SaveChanges(true);
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "Araba Silinemedi");
			}
			return RedirectToAction("Index", "Car");
		}

		#endregion


		//Araba Düzenleme İşlemleri
		#region 

		public IActionResult Edit(int id)
		{
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				return RedirectToAction("Index", "Car");
			}

			var carmodel = new CarViewModel()
			{
				Brand = car.Brand,
				Model = car.Model,
				Year = car.Year,
				FuelType = car.FuelType,
				TransmissionType = car.TransmissionType,
				DailyRate = car.DailyRate,
				Availability = car.Availability,
			};
			ViewData["CarID"] = car.CarID;
			ViewData["ImageFile"] = car.ImageFile;

			return View(carmodel);
		}

		[HttpPost]
		public IActionResult Edit(int id, CarViewModel carmodel)
		{
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				return RedirectToAction("Index", "Car");
			}

			if (ModelState.IsValid is false)
			{
				ViewData["CarId"] = car.CarID;
				ViewData["ImageFile"] = car.ImageFile;
				return View(carmodel);
			}

			string newfilename = car.ImageFile;
			if (carmodel.ImageFile != null)
			{
				newfilename = Guid.NewGuid().ToString() + "_" + carmodel.ImageFile.FileName;

				string imagefullpath = _environment.WebRootPath + "/images/" + newfilename;
				using (var stream = System.IO.File.Create(imagefullpath))
				{
					carmodel.ImageFile.CopyTo(stream);
				}
				string oldImagePath = _environment.WebRootPath + "/images/" + car.ImageFile;
				System.IO.File.Delete(oldImagePath);

			}
			car.Brand = carmodel.Brand;
			car.Year = carmodel.Year;
			car.DailyRate = carmodel.DailyRate;
			car.Model = carmodel.Model;
			car.TransmissionType = carmodel.TransmissionType;
			car.Availability = carmodel.Availability;
			car.ImageFile = newfilename;

			_context.SaveChanges();

			return RedirectToAction("Index", "Car");
		}

		#endregion


		
	}
}
