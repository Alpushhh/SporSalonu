using Microsoft.AspNetCore.Identity;

namespace SporSalonu.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } // Ad Soyad
        public int? BirthYear { get; set; }  // Doğum Yılı

        // Ödevdeki Yapay Zeka kısmı için gerekli fiziksel bilgiler
        public double? Weight { get; set; } // Kilo
        public double? Height { get; set; } // Boy
        public string? Gender { get; set; } // Cinsiyet
    }
}