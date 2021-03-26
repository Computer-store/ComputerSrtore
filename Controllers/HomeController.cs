using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeApp1.Models;

namespace WeApp1.Controllers
{
    public class HomeController : Controller
    {

        ComputerStoreClassLib.Model.Context.Products context = new ComputerStoreClassLib.Model.Context.Products();
        ComputerStoreClassLib.Model.Context.Categories c1 = new ComputerStoreClassLib.Model.Context.Categories();
        ComputerStoreClassLib.Model.Context.SupplerCompanies c2 = new ComputerStoreClassLib.Model.Context.SupplerCompanies();
        ComputerStoreClassLib.Model.Context.OperationSystems c4 = new ComputerStoreClassLib.Model.Context.OperationSystems();
        ComputerStoreClassLib.Model.Context.BasketCts c5 = new ComputerStoreClassLib.Model.Context.BasketCts();
        private UserManager<ApplicationUser> UserManager
        {
            get
            {
                return gerum();
                UserManager<ApplicationUser> gerum()
                {
                    var context = new ApplicationDbContext();
                    var rolestore = new UserStore<ApplicationUser>(context);
                    var d = new UserManager<ApplicationUser>(rolestore);
                    return d;
                }
            }

        }
        private RoleManager<ApplicationRole> RoleManager
        {
            get
            {
                return getrm();
                RoleManager<ApplicationRole> getrm()
                {
                    var context = new ApplicationDbContext();
                    var rolestore = new RoleStore<ApplicationRole>(context);
                    var d = new RoleManager<ApplicationRole>(rolestore);
                    return d;
                }
            }
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {

            return View();
        }

        [AllowAnonymous]
        public async Task< ActionResult >Contact()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Проблемы с работой сайта", Value = "0" });
            items.Add(new SelectListItem { Text = "Проблемы при оформлении заказа", Value = "1" });
            items.Add(new SelectListItem { Text = "Проблемы при возврате заказа", Value = "2" });
            items.Add(new SelectListItem { Text = "Проблемы при регистрации/авторизации", Value = "3" });
            ViewBag.Reasons = items;
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Report report = new Report();
            if (user != null)
            {
                report = new Report(user.UserName, user.Email);
            }
            return View(report);
        }
        /// <summary>
        /// здесь указывается ящик для отправки писем
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [HttpPost]
       [AllowAnonymous]
        public async Task<ActionResult> Contact(Report r)
        {

            r.ReportReason = int.Parse(Request.Form["Reasons"]);
            string d = null;
            List<string> messages = new List<string> { "Проблемы с работой сайта", "Проблемы при оформлении заказа", "Проблемы при оформлении заказа", "Проблемы при возврате заказа", "Проблемы при регистрации/авторизации" };
            MailAddress from = new MailAddress(r.Email, r.UserName);
            MailAddress to = new MailAddress("calling.znanija@gmail.com");
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = "Проблема при работе сайта";
            d = "Пользователь сайта" + r.UserName + " имеет проблему: " + messages[r.ReportReason] + ". Просьба решить ее";
            msg.Body = "<h2>" + d + "</h2>";
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            #region
            
            client.Credentials = new NetworkCredential("calling.znanijag@gmail.com", "forsag5school46");
            #endregion
            client.EnableSsl = true;
            await client.SendMailAsync(msg);
            return RedirectToAction("ThankYou");
        }

        public static int CountProducts(string uid)
        {
            var t = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ComputerStoreClassLib.Model.Context.BasketCts b = new ComputerStoreClassLib.Model.Context.BasketCts();
            int n = 0;
            if (t != null)
            {
                n = b.GetCountProductsForUser(t);
            }

            return n;

        }
    }
}