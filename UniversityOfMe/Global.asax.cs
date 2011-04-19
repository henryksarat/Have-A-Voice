using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UniversityOfMe {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "DefaultMessage", // Route name
                "Message/{action}/{id}", // URL with parameters
                new { controller = "Message", action = "Inbox" } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPhotoAlbum", // Route name
                "PhotoAlbum/{action}/{id}", // URL with parameters
                new { controller = "PhotoAlbum", action = "List" } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPhoto", // Route name
                "Photo/{action}/{id}", // URL with parameters
                new { controller = "Photo", action = "List" } // Parameter defaults
            );

            routes.MapRoute(
                "UniversityDefault", // Route name
                "{universityId}/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "User", action = "Create", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}