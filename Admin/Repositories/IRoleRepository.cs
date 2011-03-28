using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Admin.Repositories {
    public interface IRoleRepository<T, U> {
        U Create(T aCreatedByUser, U aRoleToCreate, List<int> aSelectedPermissionIds);
        U Edit(T aEditedByUser, U aRoleToEdit, List<int> aSelectedPermissionIds);
        void Delete(T aDeletedByUser, U aRoleToDelete);
        U FindRole(int id);
        IEnumerable<T> FindUsersInRole(int aRoleId);
        IEnumerable<U> GetAllRoles();
        U GetDefaultRole();
        U GetRoleByName(string aName);
        U GetNotConfirmedUserRole();
        AbstractRoleModel<U> GetAbstractNotConfirmedUserRole();
        AbstractRoleModel<U> GetAbstractDefaultRole();
        void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}