using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.AdminFeatures {
    public interface IHAVPermissionService {
        Permission FindPermission(int aPermissionId);
        IEnumerable<Permission> GetAllPermissions();
        bool Create(UserInformationModel aCreatedByUser, Permission aPermissionToCreate);
        bool Edit(UserInformationModel anEditedByUser, Permission aPermissionToEdit);
        bool Delete(UserInformationModel aDeletedByUser, Permission aPermissionToDelete);
    }
}