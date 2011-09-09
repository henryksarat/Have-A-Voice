using System.Web.Mvc;
using System.Web.Routing;
using Social.User.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Binders;
using UniversityOfMe.Models.View;

namespace UniversityOfMe {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            ModelBinders.Binders.Remove(typeof(UpdatePrivacySettingsModel<PrivacySetting>));
            ModelBinders.Binders.Add(typeof(UpdatePrivacySettingsModel<PrivacySetting>), new UpdatePrivacySettingsModelBinder());

            ModelBinders.Binders.Remove(typeof(UpdateFeaturesModel));
            ModelBinders.Binders.Add(typeof(UpdateFeaturesModel), new UpdateFeaturesModelBinder());
            ModelBinders.Binders.Remove(typeof(RoleViewModel));
            ModelBinders.Binders.Add(typeof(RoleViewModel), new RoleModelBinder());

            routes.MapRoute(
                "DefaultAuthentication", // Route name
                "Authentication/{action}/{id}", // URL with parameters
                new { controller = "Authentication", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultBoard", // Route name
                "Board/{action}/{id}", // URL with parameters
                new { controller = "Board", action = "Create", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultBoardReply", // Route name
                "BoardReply/{action}/{id}", // URL with parameters
                new { controller = "BoardReply", action = "Create", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultUserStatus", // Route name
                "UserStatus/{action}/{id}", // URL with parameters
                new { controller = "UserStatus", action = "Delete", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultMessage", // Route name
                "Message/{action}/{id}", // URL with parameters
                new { controller = "Message", action = "Inbox", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPhotoAlbum", // Route name
                "PhotoAlbum/{action}/{id}", // URL with parameters
                new { controller = "PhotoAlbum", action = "Create", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultProfile", // Route name
                "Profile/{action}/{id}", // URL with parameters
                new { controller = "Profile", action = "Show", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPhoto",
                "Photo/{action}/{id}",
                new { controller = "Photo", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DefaultPhotoComment", 
                "PhotoComment/{action}/{id}",
                new { controller = "PhotoComment", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DefaultFriend", // Route name
                "Friend/{action}/{id}", // URL with parameters
                new { controller = "Friend", action = "List", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPassword", // Route name
                "Password/{action}/{id}", // URL with parameters
                new { controller = "Password", action = "Request", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultFeature", // Route name
                "Feature/{action}/{id}", // URL with parameters
                new { controller = "Feature", action = "List", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultRole", // Route name
                "Role/{action}/{id}", // URL with parameters
                new { controller = "Role", action = "List", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultPermission", // Route name
                "Permission/{action}/{id}", // URL with parameters
                new { controller = "Permission", action = "List", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultSendItems", // Route name
                "SendItems/{action}/{id}", // URL with parameters
                new { controller = "SendItems", action = "SendItem", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultHome", // Route name
                "Home/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Main", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "UniversityDefault", // Route name
                "{universityId}/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ShortNameProfile",
                "{shortName}",
                new { controller = "Profile", action = "Show" },
                new { shortName = @"[a-zA-Z0-9\.]*" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Site", action = "Main", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}