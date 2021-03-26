using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WeApp1.Controllers
{
    
    public class CategoriesController : Controller
    {
        Context.Categories context = new Context.Categories();
        [Authorize(Roles = "Admiinstrator, Director")]
        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllCategories();
            return View(items);
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Details(string id)
        {
            var t = await context.GetCategoryById(id);
            return View(t);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admiinstrator, Director")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Create(Category collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertCategory(collection);
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

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(string id)
        {
            var item = await context.GetCategoryById(id);
            return View(item);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director")]
        public async Task<ActionResult> Edit(Category cat)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    await context.UpdateCategory(cat);
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

        // GET: Categories/Delete/5
 
        public async Task<ActionResult> Delete(string id)
        {
            await context.RemoveCategory(id);
            return RedirectToAction("Index");
        }     
    }
}
