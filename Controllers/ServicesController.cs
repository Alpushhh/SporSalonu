using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    // DİKKAT: Buradaki [Authorize] kaldırıldı. Artık herkes sayfayı görebilir.
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ServicesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Services
        // Listelemeyi HERKES görebilir (Admin, Üye veya Ziyaretçi)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null) return NotFound();

            return View(service);
        }

        // ==========================================
        // YÖNETİCİ İŞLEMLERİ (SADECE ADMIN)
        // ==========================================

        // GET: Services/Create
        [Authorize(Roles = "Admin")] // Sadece Admin ekleyebilir
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,DurationMinutes,Price")] Service service, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/img/", fileName);

                    if (!Directory.Exists(Path.Combine(wwwRootPath + "/img/")))
                    {
                        Directory.CreateDirectory(Path.Combine(wwwRootPath + "/img/"));
                    }

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    service.ImageUrl = "/img/" + fileName;
                }

                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Edit/5
        [Authorize(Roles = "Admin")] // Sadece Admin düzenleyebilir
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,DurationMinutes,Price,ImageUrl")] Service service, IFormFile? imageFile)
        {
            if (id != service.ServiceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/img/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        service.ImageUrl = "/img/" + fileName;
                    }

                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Delete/5
        [Authorize(Roles = "Admin")] // Sadece Admin silebilir
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}