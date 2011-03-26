﻿using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using Social.Generic;

namespace HaveAVoice.Models.View {
    public class RoleModel {
        public Role Role { get; set; }
        public List<int> SelectedPermissionsIds { get; set; }
        public List<Permission> AllPermissions { get; set; }

        public RoleModel(Role role) {
            Role = role;
            SelectedPermissionsIds = new List<int>();
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
    }
}
