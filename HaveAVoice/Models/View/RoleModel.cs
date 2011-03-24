using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using Social.Generic;

namespace HaveAVoice.Models.View {
    public class RoleModel {
        public Role Role { get; set; }
        public List<int> SelectedPermissionsIds { get; set; }
        public int SelectedRestrictionId { get; set; }
        public List<Restriction> AllRestrictions { get; set; }
        public List<Permission> AllPermissions { get; set; }

        public RoleModel(Role role) {
            Role = role;
            SelectedPermissionsIds = new List<int>();
            SelectedRestrictionId = 0;
            AllRestrictions = new List<Restriction>();
            AllPermissions = new List<Permission>();
            ExtractCUrrectPermissions();
            
        }

        public void ExtractCUrrectPermissions() {
            foreach (RolePermission rolePermission in Role.RolePermissions.ToList()) {
                SelectedPermissionsIds.Add(rolePermission.Permission.Id);
            }
        }

        public List<Pair<Permission, bool>> PermissionSelection() {
            return SelectionHelper.PermissionSelection(SelectedPermissionsIds.ToList<int>(), AllPermissions);
        }

        public List<Pair<Restriction, bool>> RestrictionSelection() {
            return SelectionHelper.RestrictionSelection(SelectedRestrictionId, AllRestrictions);
        }
    }
}
