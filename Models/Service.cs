using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; } // Pilates, Fitness

        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; } // Kaç dakika sürdüğü

        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; } // Ücret
    }
}