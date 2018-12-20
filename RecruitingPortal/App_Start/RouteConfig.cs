using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecruitingPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "tempDefault",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Paragon", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateContactLog",
                url: "JobSeeker/{action}/{firstItem}",
                defaults: new { controller = "JobSeeker", action = "CreateContactLog", id = "", applyId = "" }
            );
        }
    }
}
