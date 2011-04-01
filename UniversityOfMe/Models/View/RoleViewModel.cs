using System.Collections.Generic;
using System.Linq;
using BaseWebsite.Models;
using Social.Generic;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class RoleViewModel : AbstractRoleViewModel<Role, Permission> {

        public RoleViewModel(Role role) : base(role) {
            ExtractCurrectPermissions();
        }

        private void ExtractCurrectPermissions() {
            foreach (RolePermission rolePermission in Role.RolePermissions.ToList()) {
                SelectedPermissionsIds.Add(rolePermission.Permission.Id);
            }
        }

        public override List<Pair<Permission, bool>> PermissionSelection() {
            return SelectionHelper.PermissionSelection(SelectedPermissionsIds.ToList<int>(), AllPermissions);
        }
    }
}
