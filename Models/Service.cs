namespace SporSalonu.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } // Pilates, Fitness
        public int DurationMinutes { get; set; } // Süre (dk)
        public decimal Price { get; set; } // Ücret
    }
}