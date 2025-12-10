namespace SporSalonu.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public string Expertise { get; set; } // Uzmanlık (Fitness, Yoga vb.)
        public string PhotoUrl { get; set; } // Fotoğraf linki

        // Randevularla ilişki
        public ICollection<Appointment> Appointments { get; set; }
    }
}