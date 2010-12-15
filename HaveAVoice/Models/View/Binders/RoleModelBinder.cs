using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.AdminFeatures;

namespace HaveAVoice.Models.View {
    public class RoleModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVRoleService roleService = new HAVRoleService(new ModelStateWrapper(null));
            List<Permission> permissions = roleService.GetAllPermissions().ToList();

            string name = BinderHelper.GetA(bindingContext, "Name");
            string description = BinderHelper.GetA(bindingContext, "Description");
            int roleId = BinderHelper.GetAInt(bindingContext, "id");
            bool defaultRole = Boolean.Parse(BinderHelper.GetA(bindingContext, "DefaultRole"));


            Role role = Role.CreateRole(roleId, name, description, defaultRole, false);

            string mySelectedPermissionIds = BinderHelper.GetA(bindingContext, "SelectedPermissions").Trim();
            string[] mySplitIds = mySelectedPermissionIds.Split(',');
            List<int> mySelectedPermissions = new List<int>();

            foreach(string id in mySplitIds) {
                if(id != string.Empty) {
                    mySelectedPermissions.Add(Int32.Parse(id));
                }
            }

            int restrictionId = BinderHelper.GetAInt(bindingContext, "SelectedRestriction");

            RoleModel myModel = new RoleModel(role);
            myModel.SelectedPermissionsIds = mySelectedPermissions;
            myModel.SelectedRestrictionId = restrictionId;

            return myModel;
        }
    }
}
