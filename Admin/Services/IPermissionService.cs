using System.Collections.Generic;
using Social.Generic.Models;
using Social.User.Models;

namespace Social.Admin {
    public interface IPermissionService<T, U> {
        U FindPermission(int aPermissionId);
        IEnumerable<U> GetAllPermissions();
        bool Create(UserInformationModel<T> aCreatedByUser, AbstractPermissionModel<U> aPermissionToCreate);
        bool Edit(UserInformationModel<T> anEditedByUser, AbstractPermissionModel<U> aPermissionToEdit);
        bool Delete(UserInformationModel<T> aDeletedByUser, U aPermissionToDelete);
    }
}