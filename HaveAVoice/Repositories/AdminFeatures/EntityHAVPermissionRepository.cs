using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVPermissionRepository : HAVBaseRepository, IHAVPermissionRepository {
        public Permission Create(User aCreatedByUser, Permission aPermissionToCreate) {
            aPermissionToCreate.EditByUserId = aCreatedByUser.Id;
            GetEntities().AddToPermissions(aPermissionToCreate);
            GetEntities().SaveChanges();
            return aPermissionToCreate;
        }

        public Permission Edit(User anEditedByUser, Permission aPermissionToEdit) {
            var myOriginalPermission = FindPermission(aPermissionToEdit.Id);
            aPermissionToEdit.EditByUserId = anEditedByUser.Id;

            GetEntities().ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, aPermissionToEdit);
            GetEntities().SaveChanges();

            return aPermissionToEdit;
        }

        public void Delete(User aDeletedByUser, Permission aPermissionToDelete) {
            Permission myOriginalPermission = FindPermission(aPermissionToDelete.Id);
            myOriginalPermission.Deleted = true;
            myOriginalPermission.DeletedByUserId = aDeletedByUser.Id;

            GetEntities().ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, myOriginalPermission);
            GetEntities().SaveChanges();
        }

        public Permission FindPermission(int aPermissionId) {
            return (from p in GetEntities().Permissions
                    where p.Id == aPermissionId && p.Deleted == false
                    select p).FirstOrDefault();
        }

        public IEnumerable<Permission> GetAllPermissions() {
            return GetEntities().Permissions.ToList<Permission>().Where(p => p.Deleted == false);
        }
    }
}