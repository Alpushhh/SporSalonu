using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    // Bu kod, tarayıcıda /api/trainers adresine gidince çalışır
    [Route("api/trainers")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers()
        {
            // Veritabanından hocaları ve verdikleri dersleri çekiyoruz
            var trainers = await _context.Trainers
                .Include(t => t.TrainerServices)
                .ThenInclude(ts => ts.Service)
                .Select(t => new
                {
                    // Sadece gerekli bilgileri seçiyoruz (Temiz JSON için)
                    Id = t.TrainerId,
                    AdSoyad = t.FullName,
                    Uzmanlik = t.Expertise,
                    CalismaSaatleri = $"{t.WorkStartHour}:00 - {t.WorkEndHour}:00",
                    Hizmetler = t.TrainerServices.Select(ts => ts.Service.ServiceName).ToList()
                })
                .ToListAsync();

            return Ok(trainers);
        }
    }
}