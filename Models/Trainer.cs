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

        // --- EKLENMESİ GEREKEN KISIM (Müsaitlik İçin) ---
        // Örneğin: 09 (Sabah 9) - 17 (Akşam 5) arası çalışır.
        // Daha detaylı bir sistem için ayrı bir tablo gerekebilir ama ödev için bu yeterlidir.

        [Required]
        [Display(Name = "Mesai Başlangıç Saati")]
        [Range(0, 23)]
        public int WorkStartHour { get; set; } = 9; // Varsayılan 09:00

        [Required]
        [Display(Name = "Mesai Bitiş Saati")]
        [Range(0, 23)]
        public int WorkEndHour { get; set; } = 18; // Varsayılan 18:00

        // --- İLİŞKİLER ---
        public ICollection<TrainerService>? TrainerServices { get; set; }

        // Bir antrenörün randevularını görebilmesi için:
        public ICollection<Appointment>? Appointments { get; set; }
    }
}