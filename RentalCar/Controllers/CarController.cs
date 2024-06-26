﻿namespace RentalCar.Controllers
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
			// Tüm arabaları getir ve görünüme dön
			var cars = _context.Cars.ToList();
			return View(cars);
		}
		#endregion


		//Araba Ekleme İşlemleri
		#region
		public IActionResult Create()
		{
			// Araba ekleme formunu görüntüle
			return View();
		}

		[HttpPost]
		public IActionResult Create(CarModel carmodel)
		{
			// Model doğrulamasını kontrol et
			if (ModelState.IsValid)
			{
				string filename = "";
				//Resim var mı kontrol et
				if (carmodel.ImageFile != null)
				{
					// Resim dosyasını yükle ve kaydet
					String uploadfolder = Path.Combine(_environment.WebRootPath, "images");
					filename = Guid.NewGuid().ToString() + "_" + carmodel.ImageFile.FileName;
					string filepath = Path.Combine(uploadfolder, filename);
					carmodel.ImageFile.CopyTo(new FileStream(filepath, FileMode.Create));
				}

				// Yeni arabayı oluştur ve veritabanına ekle
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

				_context.Cars.Add(car); // Yeni arabayı veritabanına ekle
				int affectedrows = _context.SaveChanges(); // Değişiklikleri kaydet

				if (affectedrows == 0)
				{
					// Eğer kayıt yapılamazsa hata ekle
					ModelState.AddModelError("", "Araba Eklenemedi.");
				}
				// Araba listesine yönlendir
				return RedirectToAction("Index");
			}
			// Eğer model doğrulaması başarısız olursa carmodel'i geri döndür
			return View(carmodel);
		}

		#endregion


		//Araba Silme İşlemleri
		#region

		public IActionResult Delete(int id)
		{
			// Seçilen id'li arabayı bul
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				// Eğer araba bulunamazsa, arabaların listelendiği sayfaya yönlendir
				return RedirectToAction("Index", "Car");
			}

			// Araba resmini sil
			string filepath = _environment.WebRootPath + "images" + car.ImageFile;
			System.IO.File.Delete(filepath);

			// Arabayı veritabanından kaldır
			_context.Cars.Remove(car); // Arabayı veritabanından kaldır
			int affectedrows = _context.SaveChanges(); // Değişiklikleri kaydet

			if (affectedrows == 0)
			{
				// Eğer arabayı silmek mümkün değilse, model durumuna hata ekle
				ModelState.AddModelError("", "Araba Silinemedi");
			}

			// Araba listesine yönlendir
			return RedirectToAction("Index", "Car");
		}


		#endregion


		//Araba Düzenleme İşlemleri
		#region 

		public IActionResult Edit(int id)
		{
			// Seçilen id'li aracı bul
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				// Araba bulunamazsa arabaların listelendiği sayfaya yönlendir
				return RedirectToAction("Index", "Car");
			}

			// Araba düzenleme formunu görüntüle
			var carmodel = new CarEditModel()
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
		public IActionResult Edit(int id, CarEditModel careditmodel)
		{
			// Seçilen id'li aracı bul
			var car = _context.Cars.Find(id);
			if (car is null)
			{
				// Eğer araba bulunamazsa, arabaların listelendiği sayfaya yönlendir
				return RedirectToAction("Index", "Car");
			}

			if (ModelState.IsValid)
			{
				// Değişiklik var mı kontrol et
				bool isImageChanged = careditmodel.ImageFile != null;

				// Yeni resim yükle ve kaydet
				string newfilename = car.ImageFile;
				if (isImageChanged)
				{
					newfilename = Guid.NewGuid().ToString() + "_" + careditmodel.ImageFile.FileName;

					string imagefullpath = Path.Combine(_environment.WebRootPath, "images", newfilename);
					using (var stream = System.IO.File.Create(imagefullpath))
					{
						careditmodel.ImageFile.CopyTo(stream);
					}

					// Eski resmi sil
					string oldImagePath = Path.Combine(_environment.WebRootPath, "images", car.ImageFile);
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				// Araba bilgilerini güncelle
				car.Brand = careditmodel.Brand;
				car.Year = careditmodel.Year;
				car.DailyRate = careditmodel.DailyRate;
				car.Model = careditmodel.Model;
				car.TransmissionType = careditmodel.TransmissionType;
				car.Availability = careditmodel.Availability;
				car.ImageFile = newfilename;

				// Değişiklikleri kaydet
				_context.SaveChanges();

				// Araba listesine yönlendir
				return RedirectToAction("Index", "Car");
			}

			// ModelState.IsValid false ise, düzenleme formunu tekrar göster
			ViewData["CarId"] = car.CarID;
			ViewData["ImageFile"] = car.ImageFile;
			return View(careditmodel);
		}

		#endregion

	}
}
