using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantý Ayarý (Connection String)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (Üyelik Sistemi) Ayarlarý
// Admin ve Üye rolleri için IdentityRole ekledik
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. Þifre Zorluðunu Kaldýrma (Geliþtirme aþamasýnda kolaylýk olsun diye)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; // Rakam zorunlu deðil
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false; // Sembol zorunlu deðil
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3; // En az 3 karakter
});

// MVC Servislerini Ekleme
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata Yönetimi ve Güvenlik
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Yetkilendirme Sýralamasý (Burasý Önemli)
app.UseAuthentication(); // Kimlik Doðrulama (Login oldum mu?)
app.UseAuthorization();  // Yetkilendirme (Bu sayfaya girmeye yetkim var mý?)

// Varsayýlan Rota Ayarý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();