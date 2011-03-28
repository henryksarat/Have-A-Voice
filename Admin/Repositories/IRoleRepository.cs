using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Admin.Repositories {
    public interface IRoleRepository<T, U, V> {
        U Create(T aCreatedByUser, U aRoleToCreate, IEnumerable<int> aPermissionId);
        U Edit(T aEditedByUser, U aRoleToEdit, IEnumerable<int> aPermissionsToCreateAssociationWith, IEnumerable<int> aPermissionsToDeleteAssociationWith);
        void Delete(T aDeletedByUser, U aRoleToDelete);
        U FindRole(int id);
        IEnumerable<T> FindUsersInRole(int aRoleId);
        AbstractRoleModel<U> GetAbstractDefaultRole();
        AbstractRoleModel<U> GetAbstractRoleByName(string aName);
        IEnumerable<AbstractRolePermissionModel<V>> GetAbstractRolePermissionsByRole(U aRoleToGetPermissionsFor);
        IEnumerable<U> GetAllRoles();
        U GetRoleByName(string aName);
        void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}