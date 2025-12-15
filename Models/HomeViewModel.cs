using System.Collections.Generic;

namespace SporSalonu.Models
{
    public class HomeViewModel
    {
        // Bu sınıf, Anasayfaya hem Hizmetleri hem de Eğitmenleri
        // aynı anda göndermemizi sağlayacak bir "paket" görevi görür.
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
    }
}