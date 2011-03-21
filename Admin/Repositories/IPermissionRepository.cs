using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Admin {
    public interface IPermissionRepository<T, U> {
        U Create(T aCreatedByUser, U aPermissionToCreate);
        U Edit(T aEditedByUser, U aPermissionToEdit);
        void Delete(T aDeletedByUser, U aPermissionToDelete);
        U FindPermission(int id);
        IEnumerable<U> GetAllPermissions();
        bool nameExists(String aName);
    }
}