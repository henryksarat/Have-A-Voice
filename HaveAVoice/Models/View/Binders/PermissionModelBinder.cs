using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class PermissionModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            string name = BinderHelper.GetA(bindingContext, "Name");
            string description = BinderHelper.GetA(bindingContext, "Description");
            int permissionId = BinderHelper.GetAInt(bindingContext, "id");

            Permission permission = Permission.CreatePermission(permissionId, name, description, false);

            return new PermissionModel(permission);
        }
    }
}
