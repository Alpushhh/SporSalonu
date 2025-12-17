using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

       
        public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public AppUser Member { get; set; } 

        // antrenör
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }

        // hizmet
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        // zaman
        [Required]
        public DateTime AppointmentDate { get; set; }

        
        public bool IsConfirmed { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}