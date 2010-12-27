using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVRoleRepository : IHAVRoleRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<Role> GetAllRoles() {
            return theEntities.Roles.ToList<Role>().Where(r => r.Deleted == false);
        }

        public Role FindRole(int aRoleId) {
            return (from r in theEntities.Roles.Include("RolePermissions.Permission")
                    where r.Id == aRoleId && r.Deleted == false
                    select r).FirstOrDefault();
        }

        public Role Create(User aCreatedByUser, Role aRoleToCreate, List<int> aPermissionId, int aSelectedRestrictionId) {
            aRoleToCreate.RestrictionId = aSelectedRestrictionId;
            aRoleToCreate.EditedByUserId = aCreatedByUser.Id;
            theEntities.AddToRoles(aRoleToCreate);
            theEntities.SaveChanges();

            foreach (Int32 myPermissionId in aPermissionId) {
                RolePermission rolePermission = RolePermission.CreateRolePermission(0, aRoleToCreate.Id, myPermissionId);
                theEntities.AddToRolePermissions(rolePermission);
            }

            theEntities.SaveChanges();
            return aRoleToCreate;
        }

        public Role Edit(User anEditedByUser, Role aRoleToEdit, List<Int32> aSelectedPermissionIds, int aSelectedRestrictionId) {
            Role myOriginalRole = FindRole(aRoleToEdit.Id);
            
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissions(aRoleToEdit);
            
            UpdateRolePermissions(aRoleToEdit, aSelectedPermissionIds, myRolePermissions);

            if (aRoleToEdit.DefaultRole == true && myOriginalRole.DefaultRole != true) {
                MakeNoDefaultRole();
            }
            
            myOriginalRole.Description = aRoleToEdit.Description;
            myOriginalRole.Name = aRoleToEdit.Name;
            myOriginalRole.RestrictionId = aSelectedRestrictionId;
            myOriginalRole.EditedByUserId = anEditedByUser.Id;
            theEntities.ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            theEntities.SaveChanges();

            return aRoleToEdit;
        }

        public void Delete(User aDeletedByUser, Role aRoleToDelete) {
            var myOriginalRole = FindRole(aRoleToDelete.Id);
            myOriginalRole.Deleted = true;
            myOriginalRole.DeletedByUserId = aDeletedByUser.Id;
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissions(myOriginalRole);
            DeleteRolePermissions(myRolePermissions);

            theEntities.ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            theEntities.SaveChanges();
        }

        public Role GetDefaultRole() {
            return (from c in theEntities.Roles
                    where c.DefaultRole == true
                    select c).FirstOrDefault();
        }

        public Role GetNotConfirmedUserRole() {
            Role notConfirmedRole = (from c in theEntities.Roles
                    where c.Name == HAVConstants.NOT_CONFIRMED_USER_ROLE
                    select c).FirstOrDefault();

            if (notConfirmedRole == null)
                throw new NullReferenceException("Unable to get the Not Confirmed User Role.");

            return notConfirmedRole;
        }

        public IEnumerable<User> FindUsersInRole(int aRoleId) {
            return (from u in theEntities.Users
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    where ur.Role.Id == aRoleId
                    select u).ToList<User>();
        }

        public void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            IHAVUserRepository myUserRepo = new EntityHAVUserRepository();
            Role myMoveToRole = FindRole(aToRoleId);
            foreach (int myUserId in aUsers) {
                UserRole myOriginalUserRole = GetUserRole(myUserId, aFromRoleId);

                myOriginalUserRole.Role = myMoveToRole;
                theEntities.ApplyCurrentValues(myOriginalUserRole.EntityKey.EntitySetName, myOriginalUserRole);
            }

            theEntities.SaveChanges();
        }

        private void MakeNoDefaultRole() {
            Role originalDefaultRole = GetDefaultRole();

            if (originalDefaultRole != null)
                UpdateRoleDefaultStatus(originalDefaultRole, false);

            theEntities.SaveChanges();
        }

        private Role UpdateRoleDefaultStatus(Role aRoleToUpdate, bool anIsDefault) {
            Role originalRole = FindRole(aRoleToUpdate.Id);
            aRoleToUpdate.DefaultRole = anIsDefault;
            theEntities.ApplyCurrentValues(originalRole.EntityKey.EntitySetName, aRoleToUpdate);
            return aRoleToUpdate;
        }

        private IEnumerable<RolePermission> GetRolePermissions(Role aRoleToGetPermissionsFor) {
            return (from c in theEntities.RolePermissions
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
            theEntities.DeleteObject(myOriginalRolePermission);
        }

        private RolePermission GetRolePermission(int aRolePermissionId) {
            return (from c in theEntities.RolePermissions
                    where c.Id == aRolePermissionId
                    select c).FirstOrDefault();
        }

        private void CreateRolePermission(int aRoleId, int aPermissionId) {
            RolePermission myRolePermissionToAdd = RolePermission.CreateRolePermission(0, aRoleId, aPermissionId);
            theEntities.AddToRolePermissions(myRolePermissionToAdd);
            theEntities.SaveChanges();
        }

        private UserRole GetUserRole(int aUserId, int aRoleId) {
            return (from ur in theEntities.UserRoles
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