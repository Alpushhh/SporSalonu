using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Üyenin Kendi Randevularını Listeleme
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Eğer Admin ise tüm randevuları görsün
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var allAppointments = await _context.Appointments
                    .Include(a => a.Trainer)
                    .Include(a => a.Service)
                    .Include(a => a.Member)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();
                return View("IndexAdmin", allAppointments); // Admin için ayrı view
            }

            // Üye ise sadece kendi randevularını görsün
            var myAppointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.MemberId == user.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(myAppointments);
        }

        // 2. Yeni Randevu Alma Sayfası
        public IActionResult Create()
        {
            // Dropdownlar için verileri hazırla
            ViewBag.Trainers = new SelectList(_context.Trainers, "TrainerId", "FullName");
            ViewBag.Services = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // 3. Randevu Kaydetme (MANTIK BURADA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainerId, int serviceId, DateTime date, int hour)
        {
            // 1. Geçerli bir tarih mi?
            DateTime appointmentDateTime = date.Date.AddHours(hour);
            if (appointmentDateTime <= DateTime.Now)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // 2. Antrenör O Saatte Çalışıyor mu?
            var trainer = await _context.Trainers.FindAsync(trainerId);
            if (trainer != null)
            {
                if (hour < trainer.WorkStartHour || hour >= trainer.WorkEndHour)
                {
                    ModelState.AddModelError("", $"Bu antrenör sadece {trainer.WorkStartHour}:00 - {trainer.WorkEndHour}:00 saatleri arasında çalışmaktadır.");
                }
            }

            // 3. O Saatte Başka Randevu Var mı? (Çakışma Kontrolü)
            bool isBusy = await _context.Appointments.AnyAsync(a =>
                a.TrainerId == trainerId &&
                a.AppointmentDate == appointmentDateTime);

            if (isBusy)
            {
                ModelState.AddModelError("", "Seçtiğiniz saatte antrenör dolu. Lütfen başka bir saat seçiniz.");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var appointment = new Appointment
                {
                    MemberId = user.Id,
                    TrainerId = trainerId,
                    ServiceId = serviceId,
                    AppointmentDate = appointmentDateTime,
                    IsConfirmed = false, // Varsayılan olarak onaysız
                    CreatedDate = DateTime.Now
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Randevu talebiniz alındı, onay bekleniyor.";
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa sayfayı tekrar doldur
            ViewBag.Trainers = new SelectList(_context.Trainers, "TrainerId", "FullName");
            ViewBag.Services = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // 4. Randevu Onaylama (Sadece Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 5. Randevu Silme/İptal
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            // Sadece kendi randevusuysa veya Admin ise silebilir
            var user = await _userManager.GetUserAsync(User);

            if (appointment != null)
            {
                if (appointment.MemberId == user.Id || await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    _context.Appointments.Remove(appointment);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}