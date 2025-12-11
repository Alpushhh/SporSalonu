namespace SporSalonu.Models // Burası senin proje isminle aynı kalsın
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public string Expertise { get; set; }

        // Soru işareti (?) ekledik: Artık boş bırakılabilir
        public string? PhotoUrl { get; set; }

        // Soru işareti (?) ekledik: Artık sistem burası boş diye kızmaz
        public ICollection<Appointment>? Appointments { get; set; }
    }
}