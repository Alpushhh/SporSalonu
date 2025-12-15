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

        // DİKKAT: Google'dan aldığın API Key'i tırnakların içine yapıştır!
        private const string ApiKey = "AIzaSyBTMTUDNyK1WwMHGquY3eHUJvBTc5UvMvc";

        // Model adresini güncelledim (gemini-1.5-flash en kararlısı)
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
        public async Task<IActionResult> GetAdvice(int age, double weight, double height, string goal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.BirthYear = DateTime.Now.Year - age;
                user.Weight = weight;
                user.Height = height;
                await _userManager.UpdateAsync(user);
            }

            // Basit Prompt
            string prompt = $"Ben {age} yaşında, {weight} kg, {height} cm boyundayım. Hedefim: {goal}. Bana kısa bir spor ve beslenme tavsiyesi ver.";

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
                    // API Key'i URL'e ekliyoruz
                    var response = await client.PostAsync($"{ApiUrl}?key={ApiKey}",
                        new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(responseString))
                        {
                            var text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                            ViewBag.Result = text.Replace("*", "").Replace("\n", "<br>");
                        }
                    }
                    else
                    {
                        // HATA DETAYINI OKU
                        var errorMsg = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = $"HATA OLUŞTU! Kodu: {response.StatusCode}. <br> Detay: {errorMsg}";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Result = $"Bağlantı Hatası: {ex.Message}";
                }
            }

            return View("Index", user);
        }
    }
}