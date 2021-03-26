using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeApp1.Models;

namespace WeApp1.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var t = await Manager.Users.ToListAsync();
            return View(t);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await Manager.FindByIdAsync(id);
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel m)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = new ApplicationUser { Email = m.Email, UserName = m.UserName, Address = m.Address, PhoneNumber = m.PhoneNuber };
                var rezult = await Manager.CreateAsync(user, m.Password);
                if (rezult.Succeeded)
                {
                    var t = await Manager.FindByEmailAsync(m.Email);
                    var result = await SignInManager.PasswordSignInAsync(t.UserName, m.Password, false, shouldLockout: false);
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(rezult);
                    return View(m);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await Manager.FindByIdAsync(id);
            EditViewModel evm = new EditViewModel { Id = user.Id, Address = user.Address, Email = user.Email, UserName = user.UserName, PhoneNuber = user.PhoneNumber };
            return View(evm);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditViewModel m)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    var t = await Manager.FindByIdAsync(m.Id);
                    if (t != null)
                    {
                        t.Email = m.Email;
                        t.PhoneNumber = m.PhoneNuber;
                        t.Address = m.Address;
                        t.UserName = m.UserName;
                        t.PasswordHash = Manager.PasswordHasher.HashPassword(m.Password);
                        var r = await Manager.UpdateAsync(t);
                        if (r.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrors(r);
                            return View(t);
                        }
                    }
                    else
                    {
                        return View(t);
                    }
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

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await Manager.FindByIdAsync(id);
            if (user != null)
            {
                await Manager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        public void AddErrors(IdentityResult r)
        {
            foreach (string err in r.Errors)
            {
                ModelState.AddModelError("", err);
            }
        }
        private ApplicationUserManager Manager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
    }
}