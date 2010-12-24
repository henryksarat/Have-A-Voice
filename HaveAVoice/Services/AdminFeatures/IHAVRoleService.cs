using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public interface IHAVRoleService {
        Role FindRole(int id);
        IEnumerable<Role> GetAllRoles();
        bool Create(UserInformationModel aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        bool Edit(UserInformationModel anEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissions, int aSelectedRestrictionId);
        bool Delete(UserInformationModel aDeletedByUser, Role aRoleToDelete);
        IEnumerable<User> UsersInRole(int aRoleId);
        bool MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}