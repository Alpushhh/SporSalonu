namespace SporSalonu.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsConfirmed { get; set; } = false; // Onay durumu

        // Randevuyu alan Üye
        public string MemberId { get; set; }
        public AppUser Member { get; set; }

        // Seçilen Antrenör
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        // Seçilen Hizmet
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}