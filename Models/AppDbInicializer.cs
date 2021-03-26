using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Data.Entity;
using ComputerStoreClassLib.Model;
using System.Threading.Tasks;

namespace WeApp1.Models
{
    
    public class AppDbInicializer:DropCreateDatabaseAlways<ApplicationDbContext>
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
        protected override void Seed(ApplicationDbContext context)
        {
            //Task t = Task.Factory.StartNew(() => Init());
            //t.Wait();
            
            base.Seed(context);
        }
        private async Task Init()
        {
            IdentityResult rez = new IdentityResult();
            rez = await RoleManager.CreateAsync(new ApplicationRole("Администратор","позволяет работать с пользователями"));
            if (rez.Succeeded)
            {
                rez = await UserManager.CreateAsync(new ApplicationUser { Email = "noreply@gmail.com", UserName = "Admin" });
                if (rez.Succeeded)
                {
                    var user = await UserManager.FindByNameAsync("Admin");
                    var role = await RoleManager.FindByNameAsync("Администратор");
                    await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
            }
            rez = await RoleManager.CreateAsync(new ApplicationRole("Продавец", "Позволяет измменять состияние заказа"));
            if (rez.Succeeded)
            {
                rez = await UserManager.CreateAsync(new ApplicationUser { Email = "noreply@gmail.com", UserName = "Seller" });
                if (rez.Succeeded)
                {
                    var user = await UserManager.FindByNameAsync("Seller");
                    var role = await RoleManager.FindByNameAsync("Продавец");
                    await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
            }
            rez = await RoleManager.CreateAsync(new ApplicationRole("Главный продавец", "Позволяет полностью работать с товарами"));
            if (rez.Succeeded)
            {
                rez = await UserManager.CreateAsync(new ApplicationUser { Email = "noreply@gmail.com", UserName = "GeneralSeller" });
                if (rez.Succeeded)
                {
                    var user = await UserManager.FindByNameAsync("GeneralSeller");
                    var role = await RoleManager.FindByNameAsync("Главный продавец");
                    await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
            }
            rez = await RoleManager.CreateAsync(new ApplicationRole("Покупатель", "Позволяет добавлять товары"));
            if (rez.Succeeded)
            {
                rez = await UserManager.CreateAsync(new ApplicationUser { Email = "noreply@gmail.com", UserName = "Buyer" });
                if (rez.Succeeded)
                {
                    var user = await UserManager.FindByNameAsync("Buyer");
                    var role = await RoleManager.FindByNameAsync("Покупатель");
                    await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
            }     

        }
    }
}