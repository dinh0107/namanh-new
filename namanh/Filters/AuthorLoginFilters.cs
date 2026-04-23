using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace namanh.Filters
{
    public class AdminRoleFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var id = (FormsIdentity)filterContext.HttpContext.User.Identity;
                var ticket = id.Ticket;

                if (string.IsNullOrEmpty(ticket.UserData))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new RouteValueDictionary { { "action", "Login" }, { "controller", "Cms" } });
                    filterContext.HttpContext.Response.End();
                }
                var data = ticket.UserData.Split('|');
                filterContext.RouteData.Values["Role"] = data[0];
                if (data.Length > 1)
                {
                    filterContext.RouteData.Values["Fullname"] = data[1];
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}