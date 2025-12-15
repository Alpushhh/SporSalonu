using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu.Models
{
    public class TrainerService
    {
        // Composite Key (Birleşik Anahtar) olacak
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}