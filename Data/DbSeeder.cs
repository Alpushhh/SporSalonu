using Microsoft.AspNetCore.Identity;
using SporSalonu.Models; // AppUser sınıfını buradan alıyor

namespace SporSalonu.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Servisleri çağırıyoruz
            var userManager = service.GetService<UserManager<AppUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Rolleri Oluştur (Admin ve Member)
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // 2. Admin Kullanıcısını Oluştur
            // Ödevdeki bilgi: ogrencinumarasi@sakarya.edu.tr / Şifre: sau
            var adminEmail = "g231210029@sakarya.edu.tr"; // Kendi numaranı yazabilirsin
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Sistem Yöneticisi",
                    EmailConfirmed = true
                };

                // Kullanıcıyı oluştur
                var result = await userManager.CreateAsync(newAdmin, "sau"); // Şifre: sau

                // Admin rolünü ata
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}