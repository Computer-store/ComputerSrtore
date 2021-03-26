using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WeApp1.Controllers
{
    public class OperationSystemsController : Controller
    {

        static Context.OperationSystems context = new Context.OperationSystems();
        // GET: OperationSystems
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllOperationSystems();
            return View(items);
        }
        [AllowAnonymous]
        // GET: OperationSystems/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var c = await context.GetOperationSystemById(id);
            return View(c);
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: OperationSystems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OperationSystems/Create
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Create(OperationSystem collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.AddingOperationSystem(collection);
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: OperationSystems/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var c = await context.GetOperationSystemById(id);
           
            return View(c);
        }

        // POST: OperationSystems/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public  async Task<ActionResult> Edit(OperationSystem os)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    await context.UpdatingOperationSystem(os);
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

        // GET: OperationSystems/Delete/5
        public async Task< ActionResult> Delete(string id)
        {
            var item = await context.GetOperationSystemById(id);
            await context.RemovingOperationSystem(item);
            return RedirectToAction("Index");
         
        }

        // POST: OperationSystems/Delete/5
      
    }
}
