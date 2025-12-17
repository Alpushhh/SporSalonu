using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            // 1. Gelen veriler boş mu kontrol et
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Lütfen tüm alanları doldurun.");
                return View();
            }

            // 2. Kullanıcıyı oluştur
            var user = new AppUser
            {
                UserName = email, 
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // 3. Başarılıysa kullanıcıya "Member" rolü ver
                await _userManager.AddToRoleAsync(user, "Member");

                // 4. Otomatik giriş yap
                await _signInManager.SignInAsync(user, isPersistent: false);

                // 5. Anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // 6. Hata varsa ekrana yazdır
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "E-posta ve şifre gereklidir.");
                return View();
            }

            // Kullanıcıyı bulma
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Şifreyi kontrol etme
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Trainers"); 
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
            return View();
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}