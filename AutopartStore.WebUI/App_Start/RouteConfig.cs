using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutopartStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { controller = "Autopart", action = "List" }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Autopart", action = "List", id = UrlParameter.Optional }
            //);

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Autopart",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Autopart", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{category}",
                new { controller = "Autopart", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Autopart", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
