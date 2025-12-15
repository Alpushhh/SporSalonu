using Microsoft.AspNetCore.Identity;

namespace SporSalonu.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        // Yapay zeka ve raporlama için gerekli alanlar
        public int? BirthYear { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Gender { get; set; }
    }
}