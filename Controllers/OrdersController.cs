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
using static WeApp1.Models.OrdersModels;

namespace WeApp1.Controllers
{
    public class OrdersController : Controller
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


        ComputerStoreClassLib.Model.Context.OrderCts context = new ComputerStoreClassLib.Model.Context.OrderCts();
        ComputerStoreClassLib.Model.Context.PayMethods pmc = new ComputerStoreClassLib.Model.Context.PayMethods();
        ComputerStoreClassLib.Model.Context.DeliveryMethods dm = new ComputerStoreClassLib.Model.Context.DeliveryMethods();
        ComputerStoreClassLib.Model.Context.OrderStates os = new ComputerStoreClassLib.Model.Context.OrderStates();
        ComputerStoreClassLib.Model.Context.Products pr = new ComputerStoreClassLib.Model.Context.Products();
        ComputerStoreClassLib.Model.Context.BasketCts bas = new ComputerStoreClassLib.Model.Context.BasketCts();
        // GET: Orders
        [Authorize]
        public async Task<ActionResult> Check()
        {
            var um = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Order fake = new Order();
            var delm = await dm.GetAllDeliveryMethods();
            var paym = await pmc.GetAllPayMethods();
            SelectList s1 = new SelectList(paym, "Id", "PayMethodName");
            SelectList s2 = new SelectList(delm, "Id", "DeliverMethodName");
            bas.SetUser(um.Id);

            var t1 = await pmc.GetAllPayMethods();
            var t2 = await dm.GetAllDeliveryMethods();
            var t3 = await os.GetAllOrderStates();
            if (um != null)
            {
                fake.Address = um.Address;
                fake.PhoneNumber = um.PhoneNumber;
                fake.Email = um.Email;
                fake.UserName = um.UserName;
            }

            var tmp = await bas.GetProductsForUser();

            int count = 0;
            double cost = 0;
            if (tmp != null)
            {
                foreach (var d in tmp)
                {
                    count += d.PruductCount;
                    cost += d.IntermediateCost;
                }

                fake.Cost = cost * count;
                fake.ProductCount = count;
                ViewBag.PayMethods = s1;
                ViewBag.DeliveryMethods = s2;
                // CheckoutOrder wsp = new CheckoutOrder { UserName = fake.UserName, ProductCount = fake.ProductCount, Cost = fake.Cost, Email = fake.Email, Address = fake.Address, PhoneNumber = fake.PhoneNumber }  ;
                return View(fake);
            }
            else
            {
                return RedirectToAction("GetAllProducts", "Products");
            }
           
        }
        [HttpPost]
        [Authorize]
        public async Task <ActionResult> Check(Order order)
        {
            var delm = await dm.GetAllDeliveryMethods();
            var paym = await pmc.GetAllPayMethods();
            var um = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (Validate(order))
            {
               
                var t1 = await pmc.GetPayMethodsById(order.PayMethodId);
                var t2 = await dm.GetDeliveryMethodById(order.DeliveryMethodId);
              
                context.SetUser(um.Id);
                await context.AddIntoOrder(new Order { UserName = order.UserName, ProductCount = order.ProductCount, Cost = order.Cost, DeliveryMethodId = order.DeliveryMethodId, PayMethodId = order.PayMethodId, Address = order.Address, PhoneNumber = order.PhoneNumber, Email = order.Email });
                return RedirectToAction("GetAllProducts", "Products");
            }
            else
            {
                SelectList s1 = new SelectList(paym, "Id", "PayMethodName", order.PayMethodId);
                SelectList s2 = new SelectList(delm, "Id", "DeliverMethodName",order.DeliveryMethodId);
                ViewBag.PayMethods = s1;
                ViewBag.DeliveryMethods = s2;
                return View(order);
            }
          
            
           

            
        }

        public bool Validate (Order order)
        {
            bool a = false;
             a = (order.UserName != null) && (order.ProductCount > 0) && (order.Cost > 0) && (order.DeliveryMethodId != null) && (order.PayMethodId != null) && (order.Address != null) && (order.PhoneNumber != null) && (order.Email != null)? true : false;
            return a;
        }
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            
            context.SetUser(user.Id);
            var orderslis = await context.GetAllOrders();
            foreach (var order in orderslis)
            {
                order.OS = await os.GetOrderStatesById(order.OrderStateId);
                order.PM = await pmc.GetPayMethodsById(order.PayMethodId);
                order.DM = await dm.GetDeliveryMethodById(order.DeliveryMethodId);
                order.US = new User { Id = user.Id, UserName = user.UserName, Address = user.Address, PhoneNumber = user.PhoneNumber };
            }
            return View(orderslis);
        }
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var order = await context.GetOrderById(id);
            order.OS = await os.GetOrderStatesById(order.OrderStateId);
            order.PM = await pmc.GetPayMethodsById(order.PayMethodId);
            order.DM = await dm.GetDeliveryMethodById(order.DeliveryMethodId);
            order.US = new User { Id = user.Id, UserName = user.UserName, Address = user.Address, PhoneNumber = user.PhoneNumber };
            return View(order);
        }
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Edit(string id)
        {
            var t1 = await pmc.GetAllPayMethods();
            var t2 = await dm.GetAllDeliveryMethods();
            var t3 = await os.GetAllOrderStates();
            var order = await context.GetOrderById(id);
            var pmit = await pmc.GetPayMethodsById(order.PayMethodId);
            var dmit = await dm.GetDeliveryMethodById(order.DeliveryMethodId);
            var osit = await os.GetOrderStatesById(order.OrderStateId);
            SelectList s1 = new SelectList(t1, "Id", "PayMethodName",pmit.Id);
            SelectList s2 = new SelectList(t2, "Id", "DeliverMethodName",dmit.Id);
            SelectList s3 = new SelectList(t3, "Id", "Name",osit.Id);
            ViewBag.DeliveryMethods = s2;
            ViewBag.PayMethods = s1;
            ViewBag.OrderStates = s3;
            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = "Admiinstrator, Director, GeneralSeller, Seller")]
        public async Task<ActionResult> Edit(Order o)
        {
            if (ModelState.IsValid)
            {
                await context.EditOrder(o);
            }
            else
            {
                return View(o);
            }
           return RedirectToAction("GetAllProducts", "Products");

        }
      
      
    }
}