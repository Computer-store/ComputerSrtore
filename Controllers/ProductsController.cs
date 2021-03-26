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
    public class ProductsController : Controller
    {
        ComputerStoreClassLib.Model.Context.Products context = new ComputerStoreClassLib.Model.Context.Products();
        ComputerStoreClassLib.Model.Context.Categories c1 = new ComputerStoreClassLib.Model.Context.Categories();
        ComputerStoreClassLib.Model.Context.SupplerCompanies c2 = new ComputerStoreClassLib.Model.Context.SupplerCompanies();
        ComputerStoreClassLib.Model.Context.OperationSystems c4 = new ComputerStoreClassLib.Model.Context.OperationSystems();

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
        // GET: Products
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Index()
        {
            
            var item = await context.GetAllProducts();
            
            return View(item);
        }

       [AllowAnonymous]
        public async Task<ActionResult> UserDetails(string id)
        {
            var product = await context.GetProductById(id);
            product.SupplerCompany = await c2.GetSupplerCompanyById(product.SupplerCompanyId);
            product.Category = await c1.GetCategoryById(product.CategoryId);
            product.OperationSystem = await c4.GetOperationSystemById(product.OperationSystemId);
            return View(product);
        }
        // GET: Products/Details/5
      [AllowAnonymous]
        public async Task<ActionResult> Details(string id)
        {
            var product = await context.GetProductById(id);
            product.SupplerCompany = await c2.GetSupplerCompanyById(product.SupplerCompanyId);
            product.Category = await c1.GetCategoryById(product.CategoryId);
            product.OperationSystem = await c4.GetOperationSystemById(product.OperationSystemId);
            return View(product);
        }
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller")]
        // GET: Products/Create
        public async Task<ActionResult> Create()
        {
            var t1 = await c1.GetAllCategories();
            var t2 = await c2.GetAllSupplerCompanies();
            var t3 = await c4.GetAllOperationSystems();
          
            SelectList s1 = new SelectList(t1, "Id", "Name");
            SelectList s2 = new SelectList(t2, "Id", "Name");
            SelectList s3 = new SelectList(t3, "Id", "Name");
            ViewBag.Categories = s1;
            ViewBag.SupplerCompanies = s2;
            ViewBag.OperationSystems = s3;
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller")]
        public async Task<ActionResult> Create(Product collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertProduct(collection);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
               
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Edit(string id)
        {
            var t1 = await c1.GetAllCategories();
            var t2 = await c2.GetAllSupplerCompanies();
            var t3 = await c4.GetAllOperationSystems();
            var item = await context.GetProductById(id);
            SelectList s1 = new SelectList(t1, "Id", "Name",item.CategoryId);
            SelectList s2 = new SelectList(t2, "Id", "Name",item.SupplerCompanyId);
            SelectList s3 = new SelectList(t3, "Id", "Name",item.OperationSystemId);
            ViewBag.Categories = s1;
            ViewBag.SupplerCompanies = s2;
            ViewBag.OperationSystems = s3;

            return View(item);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Edit(Product pr)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    await context.UpdateProduct(pr);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: Products/Delete/5
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Delete(string id)
        {
            await context.RemoveProduct(id);
            return View();
        }
        //калькулятор (в разработке)
        public async Task<ActionResult> CalCulate()
        {
            var items = await context.GetAllProducts();
            List<SelectListItem> data = new List<SelectListItem>();
            int i = 0;
            foreach (var m in items)
            {
                data.Add(new SelectListItem { Text = m.Name, Value = i.ToString() });
                i++;
            }
            var t1 = await c1.GetAllCategories();
            var t2 = await c2.GetAllSupplerCompanies();
            var t3 = await c4.GetAllOperationSystems();
         
            SelectList s1 = new SelectList(t1, "Id", "Name");
            SelectList s2 = new SelectList(t2, "Id", "Name");
            SelectList s3 = new SelectList(t3, "Id", "Name");
            ViewBag.Categories = s1;
            ViewBag.Products = data;
            ViewBag.SupplerCompanies = s2;
            ViewBag.OperationSystems = s3;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Calculate(Product t)
        {
            int index = int.Parse(Request.Form["Products"]);
            int index2 = int.Parse(Request.Form["SupplerCompanies"]);
            int index3 = int.Parse(Request.Form["Categories"]);
            int index4 = int.Parse(Request.Form["OperationSystems"]);
            
            Calculator c = new Calculator();
            //c.Category = c1
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> GetAllProducts()
        {
            var item = await context.GetAllProducts();
            foreach (var t in item)
            {
                t.OperationSystem = await c4.GetOperationSystemById(t.OperationSystemId);
                t.SupplerCompany = await c2.GetSupplerCompanyById(t.SupplerCompanyId);
                t.Category = await c1.GetCategoryById(t.CategoryId);
            }
            return View(item);
        }
        [Authorize]
        public async Task<ActionResult> AddToBasket(string id)
        {
            var item = await context.GetProductById(id);
            item.OperationSystem = await c4.GetOperationSystemById(item.OperationSystemId);
            item.SupplerCompany = await c2.GetSupplerCompanyById(item.SupplerCompanyId);
            item.Category = await c1.GetCategoryById(item.CategoryId);
            return View(item);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddToBasket( Product pr)
        {
            int i = 0;
            string dfd = "";
            if (pr.AmmountProducts > 0)
            {

                var uid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(uid);
                if (user != null)
                {
                    await context.AddProductIntoBasket(pr, user.Id, user.UserName, user.Email, user.PhoneNumber, user.Address);
                    return RedirectToAction("GetAllProducts");
                }
                else
                {
                    return View(pr);
                }

            }
            else
            {
                return View(pr);
            }
          
        }
    }
}
