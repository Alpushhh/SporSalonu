using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    
    [Route("api/trainers")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers()
        {
            
            var trainers = await _context.Trainers
                .Include(t => t.TrainerServices)
                .ThenInclude(ts => ts.Service)
                .Select(t => new
                {
                    
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