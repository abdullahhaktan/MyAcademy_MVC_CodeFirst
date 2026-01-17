using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.ContactMessageDtos;
using MyAcademy_MVC_CodeFirst.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext context;
        private readonly OpenAiService openAiService;
        private readonly MailService mailService;
        private readonly HuggingFaceMailCategoryService huggingFaceMailCategoryService;
        private readonly CustomerLogService customerLogService;




        public ContactController()
        {
            context = new AppDbContext();
            openAiService = new OpenAiService();
            mailService = new MailService();
            huggingFaceMailCategoryService = new HuggingFaceMailCategoryService();
            customerLogService = new CustomerLogService();
        }

        public ActionResult Index()
        {
            ViewBag.message = TempData["message"];

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(CreateContactMessageDto createContactMessageDto)
        {

            var detectedLanguage = await openAiService.DetectLanguageAsync(createContactMessageDto.MessageBody);

            createContactMessageDto.DetectedLanguage = detectedLanguage;

            var autoReply = await openAiService.GenerateAutoReplyAsync(createContactMessageDto.MessageBody, detectedLanguage, createContactMessageDto.SenderFullName);

            await mailService.SendAsync(createContactMessageDto.SenderEmail, "Mesajınız Hakkında", autoReply);

            var category = await huggingFaceMailCategoryService.DetectCategoryAsync(createContactMessageDto.MessageBody);

            createContactMessageDto.Category = category;

            createContactMessageDto.IsReplied = false;

            var contactMessage = MvcApplication.mapperInstance.Map<ContactMessage>(createContactMessageDto);

            context.ContactMessages.Add(contactMessage);
            await context.SaveChangesAsync();

            var email = createContactMessageDto.SenderEmail;
            var action = "Kullanıcı Mail Bıraktı";
            var ipAdress = "127.0.0.1";
            var logType = "Contact";
            var description = $"{createContactMessageDto.SenderFullName} adlı kullanıcı bir mail bıraktı";
            var createdDate = DateTime.Now;

            customerLogService.WriteLog(email, action, ipAdress, logType, description, createdDate.ToString());

            TempData["message"] = "Mesajınız başarıyla gönderildi. En kısa sürede size geri dönüş yapacağız.";

            return RedirectToAction("Index");
        }

    }
}