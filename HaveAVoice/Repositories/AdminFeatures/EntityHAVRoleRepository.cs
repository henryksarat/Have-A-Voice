using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVRoleRepository : HAVBaseRepository, IHAVRoleRepository {
        public Permission CreatePermission(User aCreatedByUser, Permission aPermissionToCreate) {
            aPermissionToCreate.EditByUserId = aCreatedByUser.Id;
            GetEntities().AddToPermissions(aPermissionToCreate);
            GetEntities().SaveChanges();
            return aPermissionToCreate;
        }
        
        public IEnumerable<Permission> GetAllPermissions() {
            return GetEntities().Permissions.ToList<Permission>().Where(p => p.Deleted == false);
        }

        public Permission GetPermission(int aPermissionId) {
            return (from p in GetEntities().Permissions
                    where p.Id == aPermissionId && p.Deleted == false
                    select p).FirstOrDefault();
        }

        public void DeletePermission(User aDeletedByUser, Permission aPermissionToDelete) {
            Permission myOriginalPermission = GetPermission(aPermissionToDelete.Id);
            myOriginalPermission.Deleted = true;
            myOriginalPermission.DeletedByUserId = aDeletedByUser.Id;
            
            GetEntities().ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, myOriginalPermission);
            GetEntities().SaveChanges();
        }

        public Permission EditPermission(User anEditedByUser, Permission aPermissionToEdit) {
            var myOriginalPermission = GetPermission(aPermissionToEdit.Id);
            aPermissionToEdit.EditByUserId = anEditedByUser.Id;

            GetEntities().ApplyCurrentValues(myOriginalPermission.EntityKey.EntitySetName, aPermissionToEdit);
            GetEntities().SaveChanges();

            return aPermissionToEdit;
        }

        public IEnumerable<Role> GetAllRoles() {
            return GetEntities().Roles.ToList<Role>().Where(r => r.Deleted == false);
        }

        public Role GetRole(int aRoleId) {
            return (from r in GetEntities().Roles.Include("RolePermissions.Permission")
                    where r.Id == aRoleId && r.Deleted == false
                    select r).FirstOrDefault();
        }

        public Role CreateRole(User aCreatedByUser, Role aRoleToCreate, List<int> aPermissionId, int aSelectedRestrictionId) {
            aRoleToCreate.RestrictionId = aSelectedRestrictionId;
            aRoleToCreate.EditedByUserId = aCreatedByUser.Id;
            GetEntities().AddToRoles(aRoleToCreate);
            GetEntities().SaveChanges();

            foreach (Int32 myPermissionId in aPermissionId) {
                RolePermission rolePermission = RolePermission.CreateRolePermission(0, aRoleToCreate.Id, myPermissionId);
                GetEntities().AddToRolePermissions(rolePermission);
            }

            GetEntities().SaveChanges();
            return aRoleToCreate;
        }

        public Role EditRole(User anEditedByUser, Role aRoleToEdit, List<Int32> aSelectedPermissionIds, int aSelectedRestrictionId) {
            Role myOriginalRole = GetRole(aRoleToEdit.Id);
            
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissions(aRoleToEdit);
            
            UpdateRolePermissions(aRoleToEdit, aSelectedPermissionIds, myRolePermissions);

            if (aRoleToEdit.DefaultRole == true && myOriginalRole.DefaultRole != true) {
                MakeNoDefaultRole();
            }
            
            myOriginalRole.Description = aRoleToEdit.Description;
            myOriginalRole.Name = aRoleToEdit.Name;
            myOriginalRole.RestrictionId = aSelectedRestrictionId;
            myOriginalRole.EditedByUserId = anEditedByUser.Id;
            GetEntities().ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            GetEntities().SaveChanges();

            return aRoleToEdit;
        }

        public void DeleteRole(User aDeletedByUser, Role aRoleToDelete) {
            var myOriginalRole = GetRole(aRoleToDelete.Id);
            myOriginalRole.Deleted = true;
            myOriginalRole.DeletedByUserId = aDeletedByUser.Id;
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissions(myOriginalRole);
            DeleteRolePermissions(myRolePermissions);

            GetEntities().ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            GetEntities().SaveChanges();
        }

        public Role GetDefaultRole() {
            return (from c in GetEntities().Roles
                    where c.DefaultRole == true
                    select c).FirstOrDefault();
        }

        public Role GetNotConfirmedUserRole() {
            Role notConfirmedRole = (from c in GetEntities().Roles
                    where c.Name == HAVConstants.NOT_CONFIRMED_USER_ROLE
                    select c).FirstOrDefault();

            if (notConfirmedRole == null)
                throw new NullReferenceException("Unable to get the Not Confirmed User Role.");

            return notConfirmedRole;
        }

        public IEnumerable<User> UsersInRole(int aRoleId) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    where ur.Role.Id == aRoleId
                    select u).ToList<User>();
        }

        public void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            IHAVUserRepository myUserRepo = new EntityHAVUserRepository();
            Role myMoveToRole = GetRole(aToRoleId);
            foreach (int myUserId in aUsers) {
                UserRole myOriginalUserRole = GetUserRole(myUserId, aFromRoleId);

                myOriginalUserRole.Role = myMoveToRole;
                GetEntities().ApplyCurrentValues(myOriginalUserRole.EntityKey.EntitySetName, myOriginalUserRole);
            }

            GetEntities().SaveChanges();
        }

        private void MakeNoDefaultRole() {
            Role originalDefaultRole = GetDefaultRole();

            if (originalDefaultRole != null)
                UpdateRoleDefaultStatus(originalDefaultRole, false);

            GetEntities().SaveChanges();
        }

        private Role UpdateRoleDefaultStatus(Role aRoleToUpdate, bool anIsDefault) {
            Role originalRole = GetRole(aRoleToUpdate.Id);
            aRoleToUpdate.DefaultRole = anIsDefault;
            GetEntities().ApplyCurrentValues(originalRole.EntityKey.EntitySetName, aRoleToUpdate);
            return aRoleToUpdate;
        }

        private IEnumerable<RolePermission> GetRolePermissions(Role aRoleToGetPermissionsFor) {
            return (from c in GetEntities().RolePermissions
                    where c.Role.Id == aRoleToGetPermissionsFor.Id
                    select c);
        }

        private void DeleteRolePermissions(IEnumerable<RolePermission> aRolePermissions) {
            foreach (RolePermission myRolePermission in aRolePermissions) {
                DeleteRolePermission(myRolePermission);
            }
        }
        
        private void DeleteRolePermission(RolePermission aRolePermissionToDelete) {
            RolePermission myOriginalRolePermission = GetRolePermission(aRolePermissionToDelete.Id);
            GetEntities().DeleteObject(myOriginalRolePermission);
        }

        private RolePermission GetRolePermission(int aRolePermissionId) {
            return (from c in GetEntities().RolePermissions
                    where c.Id == aRolePermissionId
                    select c).FirstOrDefault();
        }

        private void CreateRolePermission(int aRoleId, int aPermissionId) {
            RolePermission myRolePermissionToAdd = RolePermission.CreateRolePermission(0, aRoleId, aPermissionId);
            GetEntities().AddToRolePermissions(myRolePermissionToAdd);
            GetEntities().SaveChanges();
        }

        private UserRole GetUserRole(int aUserId, int aRoleId) {
            return (from ur in GetEntities().UserRoles
                    where ur.Role.Id == aRoleId && ur.User.Id == aUserId
                    select ur).FirstOrDefault<UserRole>();
        }

        private void UpdateRolePermissions(Role roleToEdit, List<Int32> aSelectedPermissionIds, IEnumerable<RolePermission> aRolePermissions) {
            if (aRolePermissions.Count() == 0) {
                CreateAllRolePermissions(roleToEdit, aSelectedPermissionIds);
            } else if (aSelectedPermissionIds == null) {
                DeleteRolePermissions(aRolePermissions);
            } else {

                Hashtable myRolePermissionsHashTable = new Hashtable();

                foreach (RolePermission myRolePermission in aRolePermissions) {
                    myRolePermissionsHashTable.Add(myRolePermission.Permission.Id, myRolePermission); ;
                }

                foreach (RolePermission myRolePermission in aRolePermissions) {
                    if (!aSelectedPermissionIds.Contains(myRolePermission.Permission.Id))
                        DeleteRolePermission(myRolePermission);
                }

                foreach (Int32 mySelectedPermissionId in aSelectedPermissionIds) {
                    if (!myRolePermissionsHashTable.ContainsKey(mySelectedPermissionId))
                        CreateRolePermission(roleToEdit.Id, mySelectedPermissionId);
                }
            }
        }

        private void CreateAllRolePermissions(Role aRoleToEdit, List<Int32> aSelectedPermissionIds) {
            if (aSelectedPermissionIds == null) {
                return;
            }

            foreach (Int32 mySelectedPermissionId in aSelectedPermissionIds) {
                CreateRolePermission(aRoleToEdit.Id, mySelectedPermissionId);
            }
        }
    }
}