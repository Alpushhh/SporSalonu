using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonu.Models;
using System.Text;
using System.Text.Json;
using System.Net; 

namespace SporSalonu.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        
        private const string ApiKey = "apikey";

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

            
            string model = "gemini-2.5-flash"; 
            string connectionUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={ApiKey}";

            string prompt = $@"
                Sen spor hocasısın. Danışan: {gender}, {age} yaş, {weight}kg, {height}cm. Hedef: {goal}.
                Cevabı SADECE HTML (h4, ul, li) olarak ver. Markdown yok.
                1. <h4>Analiz</h4>: BMI yorumu.
                2. <h4>Antrenman</h4>: 3 madde.
                3. <h4>Beslenme</h4>: 3 madde.
            ";

            var requestBody = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(connectionUrl,
                        new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(jsonString))
                        {
                            if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                            {
                                ViewBag.Result = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                            }
                            else { ViewBag.Result = "Yapay zeka cevap veremedi."; }
                        }
                    }
                    else
                    {
                        
                        var err = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = $"API Hatası (Key veya Model): {err}";
                    }
                }
                catch (Exception ex) { ViewBag.Result = "Bağlantı Hatası: " + ex.Message; }
            }

            
            string englishGender = (gender == "Erkek") ? "man" : "woman";
            string englishGoal = "fitness";

            if (goal.Contains("Kilo")) englishGoal = "running in park";
            else if (goal.Contains("Kas")) englishGoal = "bodybuilder in gym";
            else englishGoal = "yoga in nature";

            string imagePrompt = $"realistic photo of a {englishGender} {englishGoal}, fit body, high quality";

            
            string seed = new Random().Next(1000, 9999).ToString();

            
            ViewBag.ImageUrl = $"https://image.pollinations.ai/prompt/{WebUtility.UrlEncode(imagePrompt)}?width=600&height=400&nologo=true&seed={seed}&model=flux";

            return View("Index", user);
        }
    }
}