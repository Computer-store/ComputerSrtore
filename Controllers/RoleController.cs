using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeApp1.Models;
using static WeApp1.Models.Helper;

namespace WeApp1.Controllers
{
    public class RoleController : Controller
    {
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

        // GET: Role
        [Authorize(Roles ="Administrator, Director")]
        public ActionResult Index()
        {
            var roles = RoleManager.Roles;
            return View(roles.ToList());
        }
        [Authorize(Roles = "Administrator, Director")]
        // GET: Role/Details/5
        public async Task<ActionResult> Details(string id)
        {

            var t = await RoleManager.FindByIdAsync(id);
            return View(new RoleEdit { Id = t.Id, Name = t.Name, Description = t.Description });
        }
        [Authorize(Roles = "Administrator, Director")]
        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, Director")]
        // POST: Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleCreated collection)
        {
            try
            {
                var r = new ApplicationRole { Name = collection.Name, Description = collection.Description };
                if (r != null)
                {

                    var col = await RoleManager.CreateAsync(r);
                    if (col.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(col);
                        return View();
                    }
                }
                else
                {
                    return View();
                }

                // TODO: Add insert logic here
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Administrator, Director")]
        // GET: Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            List<ApplicationUser> unmembers = new List<ApplicationUser>();
            REModificated model = new REModificated();
            model.Role = role;

            if (unmembers.ToList().Count() == 0)
            {
                unmembers = UserManager.Users.ToList();
            }
            else
            {
                foreach (var t in UserManager.Users.ToList())
                {
                    foreach (var d in unmembers)
                    {
                        if (!d.Id.Equals(t.Id))
                        {
                            unmembers.Add(t);
                            break;

                        }
                    }
                }
            }

            foreach (var d in members)
            {
                model.Members.Add(new FakeUser { Id = d.Id, Name = d.UserName });
            }
            foreach (var t in unmembers)
            {
                model.UnMembers.Add(new FakeUser { Id = t.Id, Name = t.UserName });
            }
            return View(model);
        }


        // POST: Role/Edit/5
        [HttpPost]
        [Authorize(Roles = "Administrator, Director")]
        public async Task<ActionResult> Edit(REPost model)
        {
            try
            {
                // TODO: Add update logic here
                IdentityResult r;

                if (ModelState.IsValid)
                {
                    foreach (string uid in model.IdsToAdd ?? new string[] { })
                    {
                        r = await UserManager.AddToRoleAsync(uid, model.rolename);
                        if (!r.Succeeded)
                        {
                            return View("Error", r.Errors);
                        }
                    }
                    foreach (string uis in model.IdsToDelete ?? new string[] { })
                    {
                        //id роли
                        var role = await RoleManager.FindByIdAsync(uis);
                        var user = await UserManager.FindByIdAsync(uis);

                        r = await UserManager.RemoveFromRoleAsync(user.Id, model.rolename);
                        if (!r.Succeeded)
                        {
                            return View("Error", r.Errors);
                        }
                    }

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error", "role is not found");
            }
        }
        [Authorize(Roles = "Administrator, Director")]
        // GET: Role/Delete/5
        [Authorize(Roles = "Administrator, Director")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByNameAsync(id);
            if (role != null)
            {
                var rezult = await RoleManager.DeleteAsync(role);
                if (rezult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("error", rezult.Errors);
                }
            }
            else
            {

            }
            return View();
        }

        
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}