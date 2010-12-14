using System.Collections.Generic;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Services.AdminFeatures {
    public interface IHAVRoleService {
        //Roles
        Role GetRole(int id);
        IEnumerable<Role> GetAllRoles();
        bool CreateRole(UserInformationModel aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        bool EditRole(UserInformationModel anEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissions, int aSelectedRestrictionId);
        bool DeleteRole(UserInformationModel aDeletedByUser, Role aRoleToDelete);

        //Permissions
        Permission GetPermission(int aPermissionId);
        IEnumerable<Permission> GetAllPermissions();
        bool CreatePermission(UserInformationModel aCreatedByUser, Permission aPermissionToCreate);
        bool EditPermission(UserInformationModel anEditedByUser, Permission aPermissionToEdit);
        bool DeletePermission(UserInformationModel aDeletedByUser, Permission aPermissionToDelete);

        IEnumerable<User> UsersInRole(int aRoleId);
        bool MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}