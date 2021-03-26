using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WeApp1.Controllers
{
    public class PayMethodsController : Controller
    {
        Context.PayMethods context = new Context.PayMethods();
        // GET: PayMethods
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllPayMethods();
            return View(items);
        }
        [AllowAnonymous]
        // GET: PayMethods/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var item = await context.GetPayMethodsById(id);
            return View(item);
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: PayMethods/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // POST: PayMethods/Create
        [HttpPost]
        public async Task<ActionResult> Create(PayMethod collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertPayMethod(collection);
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

        // GET: PayMethods/Edit/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(string id)
        {
            var d = await context.GetPayMethodsById(id);
            return View(d);
        }

        // POST: PayMethods/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(PayMethod pm)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    await context.UpdatePayMethod(pm);
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
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: PayMethods/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var item = await context.GetPayMethodsById(id);
            await context.DeletePayMethod(id);
            return RedirectToAction("Index");
        }

     
    
    }
}
