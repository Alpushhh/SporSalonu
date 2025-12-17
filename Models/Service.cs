using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Süre girmek zorunludur.")]
        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Ücret girmek zorunludur.")]
        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }

        [Display(Name = "Fotoğraf URL")]
        public string? ImageUrl { get; set; }

        
        public ICollection<TrainerService>? TrainerServices { get; set; }
    }
}