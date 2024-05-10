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
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Register(RegisterViewModel registermodel)
		{
			if (ModelState.IsValid)
			{
				if (_context.Users.Any(x => x.Username.ToLower() == registermodel.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(registermodel.Username), "Kullanıcı bulunuyor.");
					return View(registermodel);
				}

				User user = new()
				{
					Username = registermodel.Username,
					FullName = registermodel.FullName,
					Email = registermodel.Email,
					Password = registermodel.Password,
				};

				_context.Users.Add(user);
				int affedtecrows = _context.SaveChanges();
				if (affedtecrows == 0)
				{
					ModelState.AddModelError("", "Kullanıcı Eklenemedi.");
				}
				return RedirectToAction(nameof(Login));
			}
			return View();
		}



		#endregion

		//Giriş Yapma İşlemi
		#region

		[AllowAnonymous]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return View();
			}

		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(LoginViewModel loginmodel)
		{
			if (ModelState.IsValid)
			{
				User user = _context.Users.SingleOrDefault(x => x.Username.ToLower() == loginmodel.Username.ToLower()
				&& x.Password == loginmodel.Password);

				if (user is not null)
				{
					List<Claim> claims =
					[
						new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
						new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
						new Claim(ClaimTypes.Role, user.Role),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim("Username", user.Username)
					];
					ClaimsIdentity identity = new(claims,
						CookieAuthenticationDefaults.AuthenticationScheme);
					ClaimsPrincipal principal = new(identity);
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
				}
			}
			return View(loginmodel);
		}

		#endregion

		//Çıkış Yapma İşlemi
		#region

		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		#endregion

		//Kullancı Düzenleme İşlemi
		#region
		[Authorize(Roles = "admin")]
		public IActionResult Edit(int id)
		{
			var user = _context.Users.Find(id);
			if (user is null)
			{
				return RedirectToAction("List", "Account");
			}
			var usermodel = new UserViewModel()
			{
				FullName = user.FullName,
				Username = user.Username,
				Email = user.Email,
			};
			return View(usermodel);
		}

		[HttpPost]
		public IActionResult Edit(int id, UserViewModel usermodel)
		{
			var user = _context.Users.Find(id);
			if (user is null)
			{
				return RedirectToAction("List");
			}
			if (ModelState.IsValid is false)
			{
				return View(usermodel);
			}
			user.FullName = usermodel.FullName;
			user.Username = usermodel.Username;
			user.Email = usermodel.Email;

			int affectedrows = _context.SaveChanges();
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "İşlem başarısız");
			}

			return RedirectToAction("List");
		}

		#endregion

		//Kullanıcı Silme İşlemi
		#region
		[Authorize(Roles = "admin")]
		public IActionResult Delete(int id)
		{
			var user = _context.Users.Find(id);
			if (user is null)
			{
				return RedirectToAction("List");
			}
			_context.Users.Remove(user);
			int affectedrows = _context.SaveChanges(true);
			if (affectedrows == 0)
			{
				ModelState.AddModelError("", "Kullanıcı silinemedi");
			}
			return RedirectToAction("List");
		}
		#endregion

		//Erişim Reddedildi Sayfası
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

	}
}
