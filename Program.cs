using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// Kendi proje isminle değiştir (örn: SporSalonu veya SporSalonuYonetim)
using SporSalonu.Data;
using SporSalonu.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (Üyelik) Sistemi
// DİKKAT: Sadece burası olmalı. Başka AddDefaultIdentity veya AddAuthentication olmamalı.
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Şifre kuralları (Geliştirme için basit)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Identity sayfaları için şart

var app = builder.Build();

// Hata Yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Sıralama Önemli: Önce Kimlik Doğrulama, Sonra Yetki
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Login/Register sayfalarını aktif eder

app.Run();