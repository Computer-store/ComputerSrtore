using ComputerStoreClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WeApp1.Controllers
{
    public class SupplerCompaniesController : Controller
    {
        Context.SupplerCompanies context = new Context.SupplerCompanies();
        // GET: SupplerCompanies
        public async Task<ActionResult> Index()
        {
            var items = await context.GetAllSupplerCompanies();
            return View(items);
        }

        // GET: SupplerCompanies/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var item = await context.GetSupplerCompanyById(id);
            return View(item);
        }

        // GET: SupplerCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplerCompanies/Create
        [HttpPost]
        public async Task<ActionResult> Create(SupplerCompany collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    await context.InsertSupplerCompany(collection);
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

        // GET: SupplerCompanies/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var item = await context.GetSupplerCompanyById(id);
            return View(item);
        }

        // POST: SupplerCompanies/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(SupplerCompany sk)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    await context.UpdateSupplerCompany(sk);
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

        // GET: SupplerCompanies/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            await context.RemoveSupplerCompany(id);
            return RedirectToAction("Index");
        }

      
      
    }
}
