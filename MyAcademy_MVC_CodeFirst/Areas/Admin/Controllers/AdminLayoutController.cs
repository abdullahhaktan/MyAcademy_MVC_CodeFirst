using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class AdminLayoutController : Controller
    {
        private readonly string _apiKey;
        private HttpClient _client;
        public AdminLayoutController()
        {
            _apiKey = ConfigurationManager.AppSettings["TavilyApiKey"];
            _client = new HttpClient();
        }

        // GET: Admin/AdminLayout
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AskTavily(string query)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var requestBody = new
                {
                    api_key = _apiKey,
                    query = query,
                    search_depth = "basic", // Daha detaylı sonuç için "advanced" da yapabilirsiniz
                    include_answer = true
                };

                var response = await _client.PostAsJsonAsync("https://api.tavily.com/search", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<dynamic>();

                    // 1. Seçenek: Tavily'nin ürettiği doğrudan cevap (answer)
                    string aiAnswer = result.answer;

                    // 2. Seçenek: Eğer answer boşsa, arama sonuçlarının (results) ilkini özet olarak al
                    if (string.IsNullOrEmpty(aiAnswer) && result.results != null && result.results.Count > 0)
                    {
                        aiAnswer = result.results[0].content;
                    }

                    // Hala boşsa uyarı ver
                    if (string.IsNullOrEmpty(aiAnswer))
                    {
                        aiAnswer = "Üzgünüm, bu konu hakkında spesifik bir bilgiye ulaşılamadı.";
                    }

                    return Json(new { answer = aiAnswer });
                }

                return Json(new { answer = "API Hatası oluştu. Lütfen bağlantınızı ve API anahtarınızı kontrol edin." });
            }
            catch (Exception ex)
            {
                return Json(new { answer = "Sistem hatası: " + ex.Message });
            }
        }

    }
}