using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic.Helpers;

namespace HaveAVoice.Models.View {
    public class RoleModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            string myName = BinderHelper.GetA(bindingContext, "Name");
            string myDescription = BinderHelper.GetA(bindingContext, "Description");
            int myRoleId = BinderHelper.GetAInt(bindingContext, "id");
            bool myDefaultRole = Boolean.Parse(BinderHelper.GetA(bindingContext, "DefaultRole"));

            string mySelectedPermissionIds = BinderHelper.GetA(bindingContext, "SelectedPermissions").Trim();
            string[] mySplitIds = mySelectedPermissionIds.Split(',');
            List<int> mySelectedPermissions = new List<int>();

            foreach(string id in mySplitIds) {
                if(id != string.Empty) {
                    mySelectedPermissions.Add(Int32.Parse(id));
                }
            }

            Role myRole = Role.CreateRole(myRoleId, myName, myDescription, myDefaultRole, false);

            RoleViewModel myModel = new RoleViewModel(myRole);
            myModel.SelectedPermissionsIds = mySelectedPermissions;

            return myModel;
        }
    }
}
