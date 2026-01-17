using MyAcademy_MVC_CodeFirst.DTOs;
using MyAcademy_MVC_CodeFirst.Services;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly string userName;
        private readonly string password;
        private readonly AdminLogService adminLogService;
        public LoginController()
        {
            userName = ConfigurationManager.AppSettings["AdminUserName"];
            password = ConfigurationManager.AppSettings["AdminPassword"];
            adminLogService = new AdminLogService();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDto loginDto)
        {
            if (loginDto.UserName == userName && loginDto.Password == password)
            {
                FormsAuthentication.SetAuthCookie(loginDto.UserName, false);

                adminLogService.WriteLog("Login", "Admin Girişi", "Admin Giriş Yaptı");

                return RedirectToAction("Index", "Dashboard", new { @area = "Admin" });
            }

            else
            {
                adminLogService.WriteLog("Login Error", "Admin Girişi", "Admin Hatalı Giriş Yaptı");

                return View();
            }
        }

        public async Task<ActionResult> LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index");
        }
    }
}