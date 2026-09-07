using hailinh.DAL;
using hailinh.Migrations;
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

            ControllerBuilder.Current.DefaultNamespaces.Clear();
            ControllerBuilder.Current.DefaultNamespaces.Add("hailinh.Controllers");

            // Đồng bộ ContextKey migration sau khi đổi namespace — tránh EF chạy lại CreateTable.
            Database.SetInitializer<DataEntities>(null);
            try
            {
                using (var db = new DataEntities())
                {
                    if (db.Database.Exists())
                    {
                        db.Database.ExecuteSqlCommand(@"
IF OBJECT_ID(N'dbo.__MigrationHistory', N'U') IS NOT NULL
BEGIN
    UPDATE dbo.__MigrationHistory
    SET ContextKey = N'hailinh.Migrations.Configuration'
    WHERE ContextKey <> N'hailinh.Migrations.Configuration';
END");
                    }
                }
            }
            catch
            {
                // DB chưa sẵn sàng — để MigrateDatabaseToLatestVersion xử lý.
            }

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataEntities, Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var unitofWork = new UnitOfWork())
            {
                Application["ConfigSite"] = unitofWork.ConfigSiteRepository.GetQuery().FirstOrDefault();
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
            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    string roleString = ticket.UserData;

                    var identity = new GenericIdentity(ticket.Name);
                    var principal = new GenericPrincipal(identity, new[] { roleString });

                    HttpContext.Current.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
            }
        }
    }
}
