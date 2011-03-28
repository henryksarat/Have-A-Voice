using System.Collections.Generic;
using Social.Generic.Models;
using Social.Generic.Helpers;

namespace Social.Admin.Services {
    public interface IRoleService<T, U, V> {
        U FindRole(UserInformationModel<T> aViewingUser, int aRoleId, SocialPermission aSocialPermission);
        IEnumerable<U> GetAllRoles(UserInformationModel<T> aViewingUser);
        bool Create(UserInformationModel<T> aCreatedByUser, AbstractRoleModel<U> aRoleToCreate, List<int> aSelectedPermissionIds);
        bool Edit(UserInformationModel<T> anEditedByUser, AbstractRoleModel<U> aRoleToEdit, List<int> aSelectedPermissions);
        bool Delete(UserInformationModel<T> aDeletedByUser, U aRoleToDelete);
        bool MoveUsersToRole(UserInformationModel<T> aUserDoingMove, List<int> aUsers, int aFromRoleId, int aToRoleId);
        IEnumerable<T> UsersInRole(int aRoleId);
    }
}