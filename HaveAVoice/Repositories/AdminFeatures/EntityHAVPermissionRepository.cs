using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using Social.Admin;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVPermissionRepository : IPermissionRepository<User, Permission> {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public Permission Create(User aCreatedByUser, Permission aPermissionToCreate) {
            aPermissionToCreate.EditByUserId = aCreatedByUser.Id;
            theEntities.AddToPermissions(aPermissionToCreate);
            theEntities.SaveChanges();
            return aPermissionToCreate;
        }

        public Permission Edit(User anEditedByUser, Permission aPermissionToEdit) {
            var myOriginalPermission = FindPermission(aPermissionToEdit.Id);
            aPermissionToEdit.EditByUserId = anEditedByUser.Id;

            theEntities.ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, aPermissionToEdit);
            theEntities.SaveChanges();

            return aPermissionToEdit;
        }

        public void Delete(User aDeletedByUser, Permission aPermissionToDelete) {
            Permission myOriginalPermission = FindPermission(aPermissionToDelete.Id);
            myOriginalPermission.Deleted = true;
            myOriginalPermission.DeletedByUserId = aDeletedByUser.Id;

            theEntities.ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, myOriginalPermission);
            theEntities.SaveChanges();
        }

        public Permission FindPermission(int aPermissionId) {
            return (from p in theEntities.Permissions
                    where p.Id == aPermissionId && p.Deleted == false
                    select p).FirstOrDefault();
        }

        public IEnumerable<Permission> GetAllPermissions() {
            return theEntities.Permissions.ToList<Permission>().Where(p => p.Deleted == false).OrderBy(p => p.Name);
        }

        public bool nameExists(String aName) {
            return (from p in theEntities.Permissions
                    where p.Name == aName
                    select p).Count() > 0;
        }


        public IEnumerable<Permission> GetPermissionsForUserId(int aUserId) {
            throw new NotImplementedException();
        }
    }
}