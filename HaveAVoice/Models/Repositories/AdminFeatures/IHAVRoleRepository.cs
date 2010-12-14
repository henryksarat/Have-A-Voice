using System;
using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Models.Repositories.AdminFeatures {
    public interface IHAVRoleRepository {
        //Roles
        Role GetRole(int id);
        IEnumerable<Role> GetAllRoles();
        Role CreateRole(User aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        Role EditRole(User aEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        void DeleteRole(User aDeletedByUser, Role aRoleToDelete);
        Role GetDefaultRole();
        Role GetNotConfirmedUserRole();
        IEnumerable<User> UsersInRole(int aRoleId);
        void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);

        //Permissions
        Permission GetPermission(int id);
        IEnumerable<Permission> GetAllPermissions();
        Permission CreatePermission(User aCreatedByUser, Permission aPermissionToCreate);
        Permission EditPermission(User aEditedByUser, Permission aPermissionToEdit);
        void DeletePermission(User aDeletedByUser, Permission aPermissionToDelete);
    }
}