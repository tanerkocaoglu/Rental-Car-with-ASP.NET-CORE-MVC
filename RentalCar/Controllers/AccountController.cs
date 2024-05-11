namespace RentalCar.Controllers
{
	public class AccountController(DatabaseContext context) : Controller
	{
		private readonly DatabaseContext _context = context;

		//Kayıt Olma İşlemi
		#region

		[AllowAnonymous]
		public IActionResult Register()
		{
			// Eğer kullanıcı zaten kimlik doğrulaması yapılmışsa, ana sayfaya yönlendir
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			// Kayıt formunu görüntüle
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Register(RegisterModel registermodel)
		{
			if (ModelState.IsValid)
			{
				// Kullanıcı adı benzersiz mi kontrol et
				if (_context.Users.Any(x => x.Username.ToLower() == registermodel.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(registermodel.Username), "Kullanıcı bulunuyor.");
					return View(registermodel); // Hata varsa kayıt formunu tekrar göster
				}

				// Yeni kullanıcı oluştur ve veritabanına ekle
				User user = new()
				{
					Username = registermodel.Username,
					FullName = registermodel.FullName,
					Email = registermodel.Email,
					Password = registermodel.Password,
				};

				_context.Users.Add(user); // Kullanıcıyı veritabanına ekle
				int affectedrows = _context.SaveChanges(); // Değişiklikleri kaydet
				if (affectedrows == 0)
				{
					ModelState.AddModelError("", "Kullanıcı Eklenemedi."); // Eğer kullanıcı eklenemediyse model durumuna hata ekle
				}
				return RedirectToAction(nameof(Login)); // Kullanıcıyı oturum açma sayfasına yönlendir
			}
			// Geçersiz model durumu varsa kayıt formunu tekrar göster
			return View();
		}

		#endregion

		//Giriş Yapma İşlemi
		#region

		[AllowAnonymous]
		public IActionResult Login()
		{
			// Eğer kullanıcı zaten kimlik doğrulaması yapılmışsa, ana sayfaya yönlendir
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				// Giriş formunu görüntüle
				return View();
			}
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(LoginModel loginmodel)
		{
			if (ModelState.IsValid)
			{
				// Kullanıcı adı ve şifre ile kullanıcıyı kontrol et
				User user = _context.Users.SingleOrDefault(x => x.Username.ToLower() == loginmodel.Username.ToLower()
					&& x.Password == loginmodel.Password);

				if (user is not null) // Kullanıcı bulunduysa
				{
					// Kullanıcıya ait talepleri oluştur
					List<Claim> claims =
					[
						new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
						new(ClaimTypes.Name, user.FullName ?? string.Empty),
						new(ClaimTypes.Role, user.Role),
						new(ClaimTypes.Email, user.Email),
						new("Username", user.Username)
					];
					// Kullanıcının kimliğini oluştur
					ClaimsIdentity identity = new ClaimsIdentity(claims,
						CookieAuthenticationDefaults.AuthenticationScheme);
					// Kullanıcının kimliğini oluştur
					ClaimsPrincipal principal = new ClaimsPrincipal(identity);
					// Kullanıcının oturum açma işlemini gerçekleştir
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					// Ana sayfaya yönlendir
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı."); // Kullanıcı adı veya şifre hatalıysa, model durumuna hata ekle
				}
			}
			// Geçersiz model durumu varsa giriş formunu tekrar göster
			return View(loginmodel);
		}

		#endregion

		//Çıkış Yapma İşlemi
		#region

		public IActionResult Logout()
		{
			// Oturumu sonlandır ve çerezleri temizle
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			// Ana sayfaya yönlendir
			return RedirectToAction("Index", "Home");
		}

		#endregion

		//Kullancı Düzenleme İşlemi
		#region

		[Authorize(Roles = "admin")] // Bu Action'a sadece admin erişebilir
		public IActionResult Edit(int id)
		{
			var user = _context.Users.Find(id); // View'den gelen id kullanılarak veritabanında bu id'ye karşılık gelen kullanıcı bulunuyor
			if (user is null) // Eğer kullanıcı bulunamazsa, List action'ı çağrılıyor
			{
				return RedirectToAction("List", "Account");
			}

			// Kullanıcının bilgilerini alıp, View modeline aktarıyoruz
			var usermodel = new UserViewModel()
			{
				FullName = user.FullName,
				Username = user.Username,
				Email = user.Email,
			};
			return View(usermodel); // Kullanıcı bilgilerini düzenleme sayfasına yönlendir
		}

		[HttpPost]
		public IActionResult Edit(int id, UserViewModel usermodel)
		{
			var user = _context.Users.Find(id); // View'den gelen id kullanılarak veritabanında bu id'ye karşılık gelen kullanıcı bulunuyor
			if (user is null) // Eğer kullanıcı bulunamazsa, List action'ı çağrılıyor
			{
				return RedirectToAction("List");
			}
			if (ModelState.IsValid is false) // Eğer model geçersizse, kullanıcı bilgileriyle birlikte tekrar düzenleme sayfasına yönlendiriliyor
			{
				return View(usermodel);
			}

			// Kullanıcı bilgileri güncelleniyor
			user.FullName = usermodel.FullName;
			user.Username = usermodel.Username;
			user.Email = usermodel.Email;

			int affectedrows = _context.SaveChanges(); // Değişiklikler veritabanına kaydediliyor
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "İşlem başarısız"); // Eğer bir değişiklik yapılmamışsa, model durumuna hata ekleniyor
			}

			return RedirectToAction("List"); // Kullanıcı listesine yönlendiriliyor
		}

		#endregion

		//Kullanıcı Silme İşlemi
		#region

		[Authorize(Roles = "admin")] // Bu Action'a sadece admin erişebilir
		public IActionResult Delete(int id)
		{
			var user = _context.Users.Find(id); // View'den gelen id kullanılarak veritabanında bu id'ye karşılık gelen kullanıcı bulunuyor
			if (user is null) // Eğer kullanıcı bulunamazsa, List action'ı çağrılıyor
			{
				return RedirectToAction("List");
			}
			// Kullanıcı veritabanından kaldırılıyor
			_context.Users.Remove(user);
			int affectedrows = _context.SaveChanges(); // Değişiklikler veritabanına kaydediliyor
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "Kullanıcı silinemedi"); // Eğer bir değişiklik yapılmamışsa, model durumuna hata ekleniyor
			}
			return RedirectToAction("List"); // Kullanıcı listesine yönlendiriliyor
		}

		#endregion

		//Erişim Reddedildi Sayfası
		#region

		[AllowAnonymous] // Bu Action'a yetkilendirme gerekmez
		public IActionResult AccessDenied()
		{
			return View(); // Erişim reddedildi sayfasına yönlendir
		}

		#endregion
	}
}
