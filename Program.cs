using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (AppUser Kullanarak)
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. Şifre ve Login Ayarları
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3; // Şifre: sau
});

// YÖNLENDİRME HATASINI ÇÖZEN KOD:
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Yetkisiz girişleri buraya at
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik Doğrulama
app.UseAuthorization();  // Yetkilendirme

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin Seeder Çalıştır
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

app.Run();

// Boş Email Servisi (Hata almamak için)
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage) => Task.CompletedTask;
}