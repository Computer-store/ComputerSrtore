using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WeApp1.Controllers
{
    public class OrderStatesController : Controller
    {
        // GET: OrderStates
        Context.OrderStates context = new Context.OrderStates();
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllOrderStates();
            return View(items);
        }

        // GET: OrderStates/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(string id)
        {
            var item = await context.GetOrderStatesById(id);
            return View(item);
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: OrderStates/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admiinstrator, Director")]
        // POST: OrderStates/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrderState collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertOrderState(collection);
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

        // GET: OrderStates/Edit/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(string id)
        {
            var item = await context.GetOrderStatesById(id);
            return View(item);
        }

        // POST: OrderStates/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(OrderState collection)
        {
            try
            {
                // TODO: Add update logic here
                if  (ModelState.IsValid){
                    await context.UpdateOrderState(collection);
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

        // GET: OrderStates/Delete/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Delete(string id)
        {
            await context.RemoveOrderStates(id);
            return RedirectToAction("Index");
        }

   
     
    }
}
