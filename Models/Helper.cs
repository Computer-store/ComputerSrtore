using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeApp1.Models
{
    public class Helper
    {
        public class REPost
        {
            [Required]
            public string rolename { get; set; }
            public string[] IdsToAdd { get; set; }
            public string[] IdsToDelete { get; set; }
        }

        public class REModificated
        {
            public ApplicationRole Role { get; set; }
            public List<FakeUser> Members { get; set; }
            public List<FakeUser> UnMembers { get; set; }
            public REModificated()
            {
                Members = new List<FakeUser>();
                UnMembers = new List<FakeUser>();
            }
        }

        public class FakeUser
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Flag { get; set; } = false;
        }
    }
    public static class Context
    {

        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            var context = new ApplicationDbContext();
            var rolestore = new UserStore<ApplicationUser>(context);
            var d = new UserManager<ApplicationUser>(rolestore);
            return new MvcHtmlString(d.FindByIdAsync(id).Result.UserName);
        }
    }
}