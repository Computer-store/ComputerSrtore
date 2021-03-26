using ComputerStoreClassLib.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeApp1.Models;

namespace WeApp1.Controllers
{
    public class BasketController : Controller
    {
        private ComputerStoreClassLib.Model.Context.OperationSystems conos = new ComputerStoreClassLib.Model.Context.OperationSystems();
        private ComputerStoreClassLib.Model.Context.Categories concateg = new ComputerStoreClassLib.Model.Context.Categories();
        private ComputerStoreClassLib.Model.Context.SupplerCompanies sk = new ComputerStoreClassLib.Model.Context.SupplerCompanies();
        private static UserManager<ApplicationUser> UserManager
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
        ComputerStoreClassLib.Model.Context.BasketCts conext = new ComputerStoreClassLib.Model.Context.BasketCts();
        // GET: Basket
        [Authorize]
        public async Task< ActionResult> Index()
        {
            var userid = User.Identity.GetUserId();
            conext.SetUser(userid);
            var pr = await conext.GetProductsForUser();
            if (pr != null)
            {
                foreach (var t in pr)
                {
                    var y = await UserManager.FindByIdAsync(userid);
                    t.User = new User { Id = y.Id, Address = y.Address, UserName = y.UserName, PhoneNumber = y.PhoneNumber };
                }
                return View(pr);
            }
           else
            {
              return  RedirectToAction("GetAllProducts", "Products");
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Index(List<Basket> t)
        {
            var userid = User.Identity.GetUserId();
            conext.SetUser(userid);
            t = await conext.GetProductsForUser();
            if (t != null)
            {
               // var ord = await 
                return RedirectToAction("Check","Orders");
            }
            else
            {
                return View();
            }
           
        }
    
    }
}