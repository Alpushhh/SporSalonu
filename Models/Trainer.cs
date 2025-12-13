using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string Expertise { get; set; } // Fitness, Yoga vb.

        [Display(Name = "Fotoğraf")]
        public string? PhotoUrl { get; set; } // Resim

        // Randevular listesi ekranda görünmez ama koda dursun
        public ICollection<Appointment>? Appointments { get; set; }
    }
}