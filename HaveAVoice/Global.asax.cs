using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using Social.User.Models;
using BaseWebsite.Models;

namespace HaveAVoice {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
       
        public static void RegisterRoutes(RouteCollection routes) {
            ModelBinders.Binders.Remove(typeof(HttpPostedFileBase));
            ModelBinders.Binders.Add(typeof(HttpPostedFileBase), new ImageBinder());
            ModelBinders.Binders.Remove(typeof(RoleViewModel));
            ModelBinders.Binders.Add(typeof(RoleViewModel), new RoleModelBinder());
            ModelBinders.Binders.Remove(typeof(PhotosModel));
            ModelBinders.Binders.Add(typeof(PhotosModel), new PhotosModelBinder());
            ModelBinders.Binders.Remove(typeof(UpdatePrivacySettingsModel<PrivacySetting>));
            ModelBinders.Binders.Add(typeof(UpdatePrivacySettingsModel<PrivacySetting>), new UpdatePrivacySettingsModelBinder());
            ModelBinders.Binders.Remove(typeof(UpdateUserProfileQuestionsModel));
            ModelBinders.Binders.Add(typeof(UpdateUserProfileQuestionsModel), new UpdateUserProfileQuestionsModelBinder());

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Admin/Role", // Route name
                "Admin/Role/{action}/{id}",
                new { controller = "Role", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Admin/Permission", // Route name
                "Admin/Permission/{action}/{id}",
                new { controller = "Permission", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Admin/Restriction", // Route name
                "Admin/Restriction/{action}/{id}",
                new { controller = "Restriction", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Admin/Index",
                "Admin/{action}",
                new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                "IssueWithTitle",
                "Issue/Details/{title}",
                new { controller = "Issue", action = "Details" },
                new { title = @"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z])?[a-zA-Z]*)*$" }
            );

            routes.MapRoute(
                "Issues",
                "Issue/{action}/{id}",
                new { controller = "Issue", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ShortNameProfile", 
                "{shortName}", 
                new { controller = "Profile", action = "Show" },
                new { shortName = @"[a-zA-Z0-9\.]*" } // Parameter defaults
            );

            routes.MapRoute(
                "CannotCreateMessage", // Route name
                "Message/Create", // URL with parameters
                new { controller = "Shared", action = "PageNotFound" } // Parameter defaults
            );

            routes.MapRoute(
                "Issue", // Route name
                "Issue/{action}/{id}", // URL with parameters
                new { controller = "Issue", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            /*
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            */
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}