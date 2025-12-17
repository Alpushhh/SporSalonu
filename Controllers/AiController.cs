using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonu.Models;
using System.Text;
using System.Text.Json;

namespace SporSalonu.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        // 🛑 BURAYA YENİ ALDIĞIN API KEY'İ YAPIŞTIR
        private const string ApiKey = "AIzaSyAAlKPS9BdfnqDufi3Qc1e7Fq_2ka6DGyY";

        // Arkadaşının dediği model: gemini-1.5-flash (En güncel ve hızlı olanı)
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

        public AiController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> GetAdvice(int age, double weight, double height, string gender, string goal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.BirthYear = DateTime.Now.Year - age;
                user.Weight = weight;
                user.Height = height;
                user.Gender = gender;
                await _userManager.UpdateAsync(user);
            }

            // Prompt (İstek)
            string prompt = $@"
                Sen uzman bir spor hocasısın. Danışan bilgileri:
                Cinsiyet: {gender}, Yaş: {age}, Kilo: {weight}kg, Boy: {height}cm. Hedef: {goal}.
                
                Lütfen bu kişi için şunları SAF HTML formatında hazırla (Sadece h4, ul, li, strong etiketleri kullan. Markdown ```html blokları koyma):
                1. <h4>Vücut Analizi</h4> başlığı altında BMI yorumu.
                2. <h4>Antrenman Programı</h4> başlığı altında 3 maddelik tavsiye.
                3. <h4>Beslenme Programı</h4> başlığı altında 3 maddelik tavsiye.
            ";

            // JSON Gövdesi (Google'ın istediği format)
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync($"{ApiUrl}?key={ApiKey}",
                        new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(jsonString))
                        {
                            // Cevabı ayıklama
                            if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                            {
                                var text = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                                ViewBag.Result = text;
                            }
                            else
                            {
                                ViewBag.Result = "<div class='alert alert-warning'>Yapay zeka cevap veremedi.</div>";
                            }
                        }
                    }
                    else
                    {
                        var errorMsg = await response.Content.ReadAsStringAsync();
                        // Hata detayını ekrana basıyoruz ki görelim
                        ViewBag.Result = $"<div class='alert alert-danger'>API Hatası: {response.StatusCode} <br> {errorMsg}</div>";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Result = $"<div class='alert alert-danger'>Bağlantı Hatası: {ex.Message}</div>";
                }
            }

            return View("Index", user);
        }
    }
}