using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace hailinh
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes(new HailinhDirectRouteProvider());
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hailinh.Controllers" }
            );
        }

        // Rename leftover: namanh.dll still exposes [RoutePrefix("mms")] MmsController.
        sealed class HailinhDirectRouteProvider : DefaultDirectRouteProvider
        {
            public override IReadOnlyList<RouteEntry> GetDirectRoutes(
                ControllerDescriptor controllerDescriptor,
                IReadOnlyList<ActionDescriptor> actionDescriptors,
                IInlineConstraintResolver constraintResolver)
            {
                if (controllerDescriptor.ControllerType.Namespace != "hailinh.Controllers")
                {
                    return new RouteEntry[0];
                }
                return base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver);
            }
        }
    }
}
