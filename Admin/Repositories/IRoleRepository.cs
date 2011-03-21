using System.Collections.Generic;

namespace Social.Admin.Repositories {
    public interface IRoleRepository<T, U> {
        U Create(T aCreatedByUser, U aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        U Edit(T aEditedByUser, U aRoleToEdit, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        void Delete(T aDeletedByUser, U aRoleToDelete);
        U FindRole(int id);
        IEnumerable<T> FindUsersInRole(int aRoleId);
        IEnumerable<U> GetAllRoles();
        U GetDefaultRole();
        U GetRoleByName(string aName);
        U GetNotConfirmedUserRole();
        void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}