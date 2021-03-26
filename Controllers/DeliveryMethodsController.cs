using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WeApp1.Controllers
{
    public class DeliveryMethodsController : Controller
    {
      
        static Context.DeliveryMethods context = new Context.DeliveryMethods();
        // GET: DeliveryMethods
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllDeliveryMethods();
            return View(items);
        }
       
        // GET: DeliveryMethods/Details/5
        public async Task <ActionResult> Details(string id)
        {
            var item = await context.GetDeliveryMethodById(id);
            return View(item);
        }

        // GET: DeliveryMethods/Create
        [Authorize(Roles = "Admiinstrator, Director")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryMethods/Create
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Create(DeliveryMethod collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertDeliveryMethod(collection);
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

        // GET: DeliveryMethods/Edit/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(string id)
        {
            var item = await context.GetDeliveryMethodById(id);
            return View(item);
        }

        // POST: DeliveryMethods/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(DeliveryMethod dm)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    await context.UpdateDeliveryMethod(dm);
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

        // GET: DeliveryMethods/Delete/5
        public async Task< ActionResult>Delete(string id)
        {
            await context.DeleteDeliveryMethod(id);
            return RedirectToAction("Index");
        }
    }
}
