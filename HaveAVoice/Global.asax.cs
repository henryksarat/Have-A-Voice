using System.Web.Mvc;
using System.Web.Routing;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System.Web;

namespace HaveAVoice {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
       
        public static void RegisterRoutes(RouteCollection routes) {
            ModelBinders.Binders.Remove(typeof(HttpPostedFileBase));
            ModelBinders.Binders.Add(typeof(HttpPostedFileBase), new ImageBinder());
            ModelBinders.Binders.Remove(typeof(RoleModel));
            ModelBinders.Binders.Add(typeof(RoleModel), new RoleModelBinder());
            ModelBinders.Binders.Remove(typeof(ViewMessageModel));
            ModelBinders.Binders.Add(typeof(ViewMessageModel), new ViewMessageModelBinder());
            ModelBinders.Binders.Remove(typeof(EditUserModel));
            ModelBinders.Binders.Add(typeof(EditUserModel), new EditUserModelBinder());
            ModelBinders.Binders.Remove(typeof(IssueModel));
            ModelBinders.Binders.Add(typeof(IssueModel), new IssueModelBinder());
            ModelBinders.Binders.Remove(typeof(IssueReplyDetailsModel));
            ModelBinders.Binders.Add(typeof(IssueReplyDetailsModel), new IssueReplyDetailsModelBinder());
            ModelBinders.Binders.Remove(typeof(PhotosModel));
            ModelBinders.Binders.Add(typeof(PhotosModel), new PhotosModelBinder());
            ModelBinders.Binders.Remove(typeof(RestrictionModel));
            ModelBinders.Binders.Add(typeof(RestrictionModel), new RestrictionModelBinder());
            ModelBinders.Binders.Remove(typeof(PermissionModel));
            ModelBinders.Binders.Add(typeof(PermissionModel), new PermissionModelBinder());
            ModelBinders.Binders.Remove(typeof(ComplaintModel));
            ModelBinders.Binders.Add(typeof(ComplaintModel), new ComplaintModelBinder());

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
            /*
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "NotLoggedIn", id = UrlParameter.Optional } // Parameter defaults
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