using System.Collections.Generic;

namespace SporSalonu.Models
{
    public class HomeViewModel
    {
        
        
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
    }
}