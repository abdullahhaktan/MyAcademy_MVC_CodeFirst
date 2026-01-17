using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.GeminiDtos;
using MyAcademy_MVC_CodeFirst.DTOs.PolicyDtos;
using MyAcademy_MVC_CodeFirst.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Content = MyAcademy_MVC_CodeFirst.DTOs.GeminiDtos.Content;
using Part = MyAcademy_MVC_CodeFirst.DTOs.GeminiDtos.Part;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [AllowAnonymous]
    public class PolicyController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly AdminLogService adminLogService;
        private const string Model = "gemini-2.5-flash";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/";

        public PolicyController()
        {
            _client = new HttpClient();
            _apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
            adminLogService = new AdminLogService();
        }

        public async Task<ActionResult> Index()
        {
            var values = await context.Policies.Where(c => c.IsActive == true)
                .ToListAsync();

            var policies = MvcApplication.mapperInstance
                .Map<List<ResultPolicyDto>>(values);

            return View(policies);
        }

        [HttpGet]
        public async Task<ActionResult> CreatePolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePolicy(CreatePolicyDto createPolicyDto)
        {
            var policy = MvcApplication.mapperInstance
                .Map<Policy>(createPolicyDto);

            context.Policies.Add(policy);

            adminLogService.WriteLog("Create", "Admin bir poliçe oluşturdu", $"Admin {policy.InsuranceType} adında bir poliçe oluşturdu");

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> UpdatePolicy(int id)
        {
            var value = await context.Policies.FindAsync(id);

            var policy = MvcApplication.mapperInstance
                .Map<GetPolicyByIdDto>(value);

            return View(policy);
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePolicy(UpdatePolicyDto updatePolicyDto)
        {
            var policy = await context.Policies.FindAsync(updatePolicyDto.Id);

            MvcApplication.mapperInstance
                .Map(updatePolicyDto, policy);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeletePolicy(int id)
        {
            var policy = await context.Policies.FindAsync(id);

            context.Policies.Remove(policy);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<string> GetGeminiDataAsync(
            string selectedInsuranceType,
            string selectedPremiumAmount)
        {
            string fullPrompt = $@"
            Sen profesyonel bir sigorta danışmanısın. Poliçe açıklaması üret direkt poliçe açıklamasını yap extra açıklama yapma ve * gibi şeyler ekleme direkt 2-3 paragraf halinde ver.

            Sigorta Türü: {selectedInsuranceType}
            Prim: {selectedPremiumAmount} TL
            ";

            var requestBody = new GeminiRequestDto
            {
                contents = new List<Content>
                {
                    new Content
                    {
                        role = "user",
                        parts = new List<Part>
                        {
                            new Part { text = fullPrompt }
                        }
                    }
                },
                generationConfig = new GenerationConfig
                {
                    temperature = 1f,
                    maxOutputTokens = 5000
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(
                $"{BaseUrl}{Model}:generateContent?key={_apiKey}",
                content);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var responseString = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponseDto>(responseString);

            return geminiResponse?
                       .candidates?
                       .FirstOrDefault()?
                       .content?
                       .parts?
                       .FirstOrDefault()?
                       .text
                   ?? "Yanıt alınamadı";
        }

        [HttpPost]
        public async Task<ActionResult> GetAiSuggestion(
            string selectedEndDate,
            string selectedStartDate,
            string selectedInsuranceType,
            string selectedPremiumAmount)
        {
            var result = await GetGeminiDataAsync(
                selectedInsuranceType,
                selectedPremiumAmount);

            return Content(result);
        }
    }
}
