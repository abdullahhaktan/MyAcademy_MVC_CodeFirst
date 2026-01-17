using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class HuggingFaceMailCategoryService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public HuggingFaceMailCategoryService()
        {
            _apiKey = ConfigurationManager.AppSettings["HuggingFaceToken"];
            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("HuggingFace API anahtarı AppSettings içinde bulunamadı.");

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _client.DefaultRequestHeaders.Add("User-Agent", "MyAcademy/1.0");
        }

        public async Task<string> DetectCategoryAsync(string message)
        {
            string translatedInput = await TranslateText(message, "Helsinki-NLP/opus-mt-tr-en");
            if (string.IsNullOrWhiteSpace(translatedInput)) return "Mesaj çevrilemedi.";

            var labelMapping = new Dictionary<string, string>
            {
                { "ThankYou", "Teşekkür ve Olumlu Görüş" },
                { "Request", "Rica ve Öneri" },
                { "Complaint", "Şikayet ve Problem Bildirimi" },
                { "Discount", "İndirim ve Kampanya Talebi" },
                { "Support", "Müşteri Destek Talebi" },
                { "Job", "İş ve Kariyer Başvurusu" },
                { "Technical", "Teknik Destek ve Servis" },
                { "Information", "Genel Bilgi Talebi" }
            };

            var payload = new
            {
                inputs = translatedInput,
                parameters = new { candidate_labels = labelMapping.Keys.ToArray(), multi_label = false }
            };

            try
            {
                string url = "https://router.huggingface.co/hf-inference/models/facebook/bart-large-mnli";
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString.Contains("currently loading"))
                    return "Model hazırlanıyor, lütfen tekrar deneyin.";

                if (!response.IsSuccessStatusCode)
                    return $"Hata: {response.StatusCode}";

                var token = Newtonsoft.Json.Linq.JToken.Parse(responseString);

                // JSON dizisini güvenli şekilde oku
                if (token is Newtonsoft.Json.Linq.JArray array && array.Count > 0)
                {
                    string bestMatch = array[0]["label"]?.ToString();

                    if (!string.IsNullOrEmpty(bestMatch) && labelMapping.ContainsKey(bestMatch))
                    {
                        return labelMapping[bestMatch];
                    }
                }

                return "Genel Kategori";
            }
            catch (Exception ex)
            {
                return "Sistem Hatası: " + ex.Message;
            }
        }

        // Ortak Çeviri Metodu (Hem TR-EN hem EN-TR için kullanılabilir)
        private async Task<string> TranslateText(string text, string modelName)
        {
            try
            {
                string url = $"https://router.huggingface.co/hf-inference/models/{modelName}";
                var payload = new { inputs = text };
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                var token = Newtonsoft.Json.Linq.JToken.Parse(result);
                // Çeviri sonucunu güvenli şekilde al
                return token.SelectToken("..translation_text")?.ToString() ?? text;
            }
            catch
            {
                return text;
            }
        }

    }
}