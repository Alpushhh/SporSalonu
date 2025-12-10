using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Models;

namespace SporSalonu.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        // BU KISMI EKLİYORUZ: Veritabanı oluşurken çalışacak kodlar
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Rolleri Tanımla (Admin ve Uye)
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Uye", NormalizedName = "UYE" }
            );

            // 2. Admin Kullanıcısını Oluştur
            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "a1",
                    UserName = "g231210029@sakarya.edu.tr", // BURAYA KENDİ NUMARANI YAZ
                    NormalizedUserName = "G231210029@SAKARYA.EDU.TR", // BURAYA KENDİ NUMARANI YAZ (BÜYÜK HARFLE)
                    Email = "g232210029@sakarya.edu.tr", // BURAYA KENDİ NUMARANI YAZ
                    NormalizedEmail = "G231210029@SAKARYA.EDU.TR", // BURAYA KENDİ NUMARANI YAZ (BÜYÜK HARFLE)
                    EmailConfirmed = true,
                    FullName = "Admin Kullanıcı",
                    PasswordHash = hasher.HashPassword(null, "sau") // Şifre: sau
                }
            );

            // 3. Kullanıcıya Admin Rolünü Ata
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId = "a1" }
            );

            // Decimal uyarısını düzeltmek için (Opsiyonel ama temiz kod için)
            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}