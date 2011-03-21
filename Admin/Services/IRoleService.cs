using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Admin.Services {
    public interface IRoleService<T, U> {
        U FindRole(int id);
        IEnumerable<U> GetAllRoles();
        bool Create(UserInformationModel<T> aCreatedByUser, AbstractRoleModel<U> aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        bool Edit(UserInformationModel<T> anEditedByUser, AbstractRoleModel<U> aRoleToEdit, List<int> aSelectedPermissions, int aSelectedRestrictionId);
        bool Delete(UserInformationModel<T> aDeletedByUser, U aRoleToDelete);
        IEnumerable<T> UsersInRole(int aRoleId);
        bool MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}