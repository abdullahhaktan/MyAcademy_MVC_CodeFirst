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
            apiKey = ConfigurationManager.AppSettings["OpenAiApiKey"];
        }

        public async Task<string> AskAsync(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    return "OpenAI isteği başarısız.";
                }

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseString);

                return result.choices[0].message.content.ToString();
            }
        }


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

            return result.Trim();

        }

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


            var reply = await AskAsync(prompt);
            return reply.Trim();
        }

    }
}