using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Models;

namespace SporSalonu.Data
{
    // DİKKAT: IdentityDbContext<AppUser> olmalı
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TrainerService> TrainerServices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Çoka-Çok İlişki (Antrenör - Hizmet)
            builder.Entity<TrainerService>()
                .HasKey(ts => new { ts.TrainerId, ts.ServiceId });

            builder.Entity<TrainerService>()
                .HasOne(ts => ts.Trainer)
                .WithMany(t => t.TrainerServices)
                .HasForeignKey(ts => ts.TrainerId);

            builder.Entity<TrainerService>()
                .HasOne(ts => ts.Service)
                .WithMany(s => s.TrainerServices)
                .HasForeignKey(ts => ts.ServiceId);
        }
    }
}