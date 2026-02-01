using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.ContactMessageDtos;
using MyAcademy_MVC_CodeFirst.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    [AllowAnonymous] // Allow access without authentication
    public class ContactController : Controller
    {
        // Service dependencies for contact form processing
        private readonly AppDbContext context;
        private readonly OpenAiService openAiService;
        private readonly MailService mailService;
        private readonly HuggingFaceMailCategoryService huggingFaceMailCategoryService;
        private readonly CustomerLogService customerLogService;

        public ContactController()
        {
            // Initialize services (consider using Dependency Injection instead)
            context = new AppDbContext();
            openAiService = new OpenAiService();
            mailService = new MailService();
            huggingFaceMailCategoryService = new HuggingFaceMailCategoryService();
            customerLogService = new CustomerLogService();
        }

        // Display contact form
        public ActionResult Index()
        {
            ViewBag.message = TempData["message"]; // Show success/error messages
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(CreateContactMessageDto createContactMessageDto)
        {
            // Step 1: Detect language of the message using OpenAI
            var detectedLanguage = await openAiService.DetectLanguageAsync(createContactMessageDto.MessageBody);
            createContactMessageDto.DetectedLanguage = detectedLanguage;

            // Step 2: Generate automated reply in detected language
            var autoReply = await openAiService.GenerateAutoReplyAsync(
                createContactMessageDto.MessageBody,
                detectedLanguage,
                createContactMessageDto.SenderFullName
            );

            // Step 3: Send auto-reply email to sender
            await mailService.SendAsync(
                createContactMessageDto.SenderEmail,
                "Mesajınız Hakkında",
                autoReply
            );

            // Step 4: Categorize message using Hugging Face AI
            var category = await huggingFaceMailCategoryService.DetectCategoryAsync(createContactMessageDto.MessageBody);
            createContactMessageDto.Category = category;

            // Step 5: Mark as not manually replied yet
            createContactMessageDto.IsReplied = false;

            // Step 6: Map DTO to entity and save to database
            var contactMessage = MvcApplication.mapperInstance.Map<ContactMessage>(createContactMessageDto);
            context.ContactMessages.Add(contactMessage);
            await context.SaveChangesAsync();

            // Step 7: Log customer action for analytics
            var email = createContactMessageDto.SenderEmail;
            var action = "Kullanıcı Mail Bıraktı";
            var ipAdress = "127.0.0.1"; // Hardcoded - consider getting real IP
            var logType = "Contact";
            var description = $"{createContactMessageDto.SenderFullName} adlı kullanıcı bir mail bıraktı";
            var createdDate = DateTime.Now;

            customerLogService.WriteLog(email, action, ipAdress, logType, description, createdDate.ToString());

            // Step 8: Show success message to user
            TempData["message"] = "Mesajınız başarıyla gönderildi. En kısa sürede size geri dönüş yapacağız.";

            return RedirectToAction("Index");
        }
    }
}