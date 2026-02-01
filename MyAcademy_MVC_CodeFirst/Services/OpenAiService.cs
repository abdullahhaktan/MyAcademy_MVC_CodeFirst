using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class OpenAiService
    {
        private readonly string apiKey = "ApiKey";

        public OpenAiService()
        {
            // Load OpenAI API key from application configuration
            apiKey = ConfigurationManager.AppSettings["OpenAiApiKey"];
        }

        // Generic method to send prompt to OpenAI and get response
        public async Task<string> AskAsync(string prompt)
        {
            using (var client = new HttpClient())
            {
                // Set authorization header with API key
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                // Prepare request body for OpenAI Chat API
                var requestBody = new
                {
                    model = "gpt-4o-mini", // Using smaller, cost-effective model
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                // Serialize request to JSON
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to OpenAI API
                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content
                );

                // Handle unsuccessful responses
                if (!response.IsSuccessStatusCode)
                {
                    return "OpenAI isteği başarısız."; // "OpenAI request failed"
                }

                // Parse and extract response content
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseString);

                return result.choices[0].message.content.ToString();
            }
        }

        // Detect language of given text using OpenAI
        public async Task<string> DetectLanguageAsync(string message)
        {
            var prompt = $@"
            Aşağıdaki metnin dilini tespit et.
            Sadece dil adını tek kelime olarak yaz.
            Açıklama yapma.
            Metin:
            {message}
            ";

            var result = await AskAsync(prompt);

            return result.Trim(); // Clean whitespace
        }

        // Generate automated reply for customer messages
        public async Task<string> GenerateAutoReplyAsync(string userMessage, string language, string senderFullName)
        {
            var prompt = $@"
                Sen bir sigorta şirketinin otomatik mail asistanısın.

                TON VE AMAÇ:
                - Kurumsal
                - Güven verici
                - Süreç başlatılmış hissi veren

                ZORUNLU KURALLAR:
                - Fiyat, indirim, kampanya veya poliçe detayları hakkında kesin bilgi verme
                - Yüzde, tutar, garanti, onay gibi ifadeler kullanma
                - Olumsuz veya savunmacı cümleler kurma (ör: 'bilgi veremeyiz')
                - Cevap mutlaka şu anlamı içersin:
                  'Talebiniz alınmıştır ve ilgili birime iletilmiştir. En kısa sürede dönüş yapılacaktır.'

                DİL:
                - Mesajın dili: {language}
                - Alıcı Ad Soyad:{senderFullName}
                - Şirket Adı: Haktan Sigorta Acente
                - Cevap aynı dilde yazılacak

                FORMAT:
                - Kısa
                - Nazik
                - Profesyonel
                - Mail dili

                Örneğin:
                Sayın Latif Çetinkaya,

                Talebiniz alınmıştır ve ilgili birime iletilmiştir. En kısa sürede dönüş yapılacaktır.

                İlginiz için teşekkür ederiz.

                Saygılarımızla, 
                Haktan Sigorta Acente - Otomatik Mail Asistanı

                Kullanıcı mesajı:
                {userMessage}
                ";

            // Get AI-generated reply
            var reply = await AskAsync(prompt);
            return reply.Trim();
        }
    }
}