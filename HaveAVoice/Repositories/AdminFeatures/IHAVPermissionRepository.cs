using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public interface IHAVPermissionRepository {
        Permission Create(User aCreatedByUser, Permission aPermissionToCreate);
        Permission Edit(User aEditedByUser, Permission aPermissionToEdit);
        void Delete(User aDeletedByUser, Permission aPermissionToDelete);
        Permission FindPermission(int id);
        IEnumerable<Permission> GetAllPermissions();
        bool nameExists(String aName);
    }
}