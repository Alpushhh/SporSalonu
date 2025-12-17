using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string Expertise { get; set; }

        [Display(Name = "Fotoğraf URL")]
        public string? PhotoUrl { get; set; }

        
        [Required]
        [Display(Name = "Mesai Başlangıç Saati")]
        [Range(0, 23)]
        public int WorkStartHour { get; set; } = 9; 

        [Required]
        [Display(Name = "Mesai Bitiş Saati")]
        [Range(0, 23)]
        public int WorkEndHour { get; set; } = 18; 

        
        public ICollection<TrainerService>? TrainerServices { get; set; }

        
        public ICollection<Appointment>? Appointments { get; set; }
    }
}