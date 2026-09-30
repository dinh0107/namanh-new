using hailinh.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace hailinh
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
              ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            Database.SetInitializer<DataEntities>(null);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var unitofWork = new UnitOfWork())
            {
                Application["ConfigSite"] = unitofWork.ConfigSiteRepository.GetQuery().FirstOrDefault();
                try
                {
                    unitofWork.CarServiceRepository.GetQuery(a => a.Active).Take(1).ToList();
                }
                catch { }
            }
        }
        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                return;
            }

            FormsAuthenticationTicket ticket;
            try
            {
                ticket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                // Cookie cũ / machineKey đổi — bỏ qua, coi như chưa đăng nhập
                return;
            }

            if (ticket == null || ticket.Expired || string.IsNullOrEmpty(ticket.Name))
            {
                return;
            }

            var roleString = ticket.UserData ?? string.Empty;
            var identity = new GenericIdentity(ticket.Name, "Forms");
            var principal = new GenericPrincipal(identity, string.IsNullOrEmpty(roleString) ? new string[0] : new[] { roleString });

            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (string.Equals(custom, "IsAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return context?.User?.Identity?.IsAuthenticated == true ? "Admin" : "Anonymous";
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }
}
