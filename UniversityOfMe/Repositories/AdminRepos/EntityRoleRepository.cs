using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Admin.Repositories;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Repositories.AdminRepos {
    public class EntityRoleRepository : IRoleRepository<User, Role, RolePermission> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Role Create(User aCreatedByUser, Role aRoleToCreate, IEnumerable<int> aPermissionId) {
            aRoleToCreate.EditedByUserId = aCreatedByUser.Id;
            theEntities.AddToRoles(aRoleToCreate);
            theEntities.SaveChanges();

            foreach (int myPermissionId in aPermissionId) {
                CreateRolePermissionWithoutSave(aRoleToCreate.Id, myPermissionId);
            }

            theEntities.SaveChanges();
            return aRoleToCreate;
        }

        public Role Edit(User anEditedByUser, Role aRoleToEdit, IEnumerable<int> aPermissionsToCreateAssociationWith, IEnumerable<int> aPermissionsToDeleteAssociationWith) {
            Role myOriginalRole = FindRole(aRoleToEdit.Id);

            DeleteRolePermissionsWithoutSave(aRoleToEdit, aPermissionsToDeleteAssociationWith);
            CreateAllRolePermissions(aRoleToEdit, aPermissionsToCreateAssociationWith);

            if (aRoleToEdit.DefaultRole == true && myOriginalRole.DefaultRole != true) {
                MakeNoDefaultRole();
            }

            myOriginalRole.Description = aRoleToEdit.Description;
            myOriginalRole.Name = aRoleToEdit.Name;
            myOriginalRole.EditedByUserId = anEditedByUser.Id;
            theEntities.ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            theEntities.SaveChanges();

            return aRoleToEdit;
        }

        public void Delete(User aDeletedByUser, Role aRoleToDelete) {
            var myOriginalRole = FindRole(aRoleToDelete.Id);
            myOriginalRole.Deleted = true;
            myOriginalRole.DeletedByUserId = aDeletedByUser.Id;
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissionsByRole(myOriginalRole);
            DeleteRolePermissionsWithoutSave(myRolePermissions);

            theEntities.ApplyCurrentValues(myOriginalRole.EntityKey.EntitySetName, myOriginalRole);
            theEntities.SaveChanges();
        }

        public Role FindRole(int aRoleId) {
            return (from r in theEntities.Roles.Include("RolePermissions.Permission")
                    where r.Id == aRoleId && r.Deleted == false
                    select r).FirstOrDefault();
        }

        public IEnumerable<User> FindUsersInRole(int aRoleId) {
            return (from u in theEntities.Users
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    where ur.Role.Id == aRoleId
                    select u).ToList<User>();
        }

        public AbstractRoleModel<Role> GetAbstractDefaultRole() {
            return SocialRoleModel.Create(GetDefaultRole());
        }

        public AbstractRoleModel<Role> GetAbstractRoleByName(string aName) {
            return SocialRoleModel.Create(GetRoleByName(aName));
        }

        public IEnumerable<AbstractRolePermissionModel<RolePermission>> GetAbstractRolePermissionsByRole(Role aRoleToGetPermissionsFor) {
            IEnumerable<RolePermission> myRolePermissions = GetRolePermissionsByRole(aRoleToGetPermissionsFor);
            return myRolePermissions.Select(r => SocialRolePermissionModel.Create(r));
        }

        public IEnumerable<Role> GetAllRoles() {
            return theEntities.Roles.ToList<Role>().Where(r => r.Deleted == false);
        }

        public Role GetRoleByName(string aName) {
            return (from r in theEntities.Roles
                    where r.Name == aName
                    select r).FirstOrDefault<Role>();
        }

        public void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            IUofMeUserRepository myUserRepo = new EntityUserRepository();
            Role myMoveToRole = FindRole(aToRoleId);
            foreach (int myUserId in aUsers) {
                UserRole myOriginalUserRole = GetUserRole(myUserId, aFromRoleId);

                myOriginalUserRole.Role = myMoveToRole;
                theEntities.ApplyCurrentValues(myOriginalUserRole.EntityKey.EntitySetName, myOriginalUserRole);
            }

            theEntities.SaveChanges();
        }

        private IEnumerable<RolePermission> GetRolePermissionsByRole(Role aRoleToGetPermissionsFor) {
            return (from c in theEntities.RolePermissions
                    where c.Role.Id == aRoleToGetPermissionsFor.Id
                    select c);
        }

        private void MakeNoDefaultRole() {
            Role myOriginalDefaultRole = GetDefaultRole();

            if (myOriginalDefaultRole != null) {
                UpdateRoleDefaultStatus(myOriginalDefaultRole, false);
            }

            theEntities.SaveChanges();
        }

        private Role GetDefaultRole() {
            return (from c in theEntities.Roles
                    where c.DefaultRole == true
                    select c).FirstOrDefault();
        }

        private RolePermission GetRolePermission(int aRolePermissionId) {
            return (from c in theEntities.RolePermissions
                    where c.Id == aRolePermissionId
                    select c).FirstOrDefault();
        }

        private UserRole GetUserRole(int aUserId, int aRoleId) {
            return (from ur in theEntities.UserRoles
                    where ur.Role.Id == aRoleId && ur.User.Id == aUserId
                    select ur).FirstOrDefault<UserRole>();
        }

        private Role UpdateRoleDefaultStatus(Role aRoleToUpdate, bool anIsDefault) {
            Role originalRole = FindRole(aRoleToUpdate.Id);
            aRoleToUpdate.DefaultRole = anIsDefault;
            theEntities.ApplyCurrentValues(originalRole.EntityKey.EntitySetName, aRoleToUpdate);
            return aRoleToUpdate;
        }

        private void DeleteRolePermissionsWithoutSave(IEnumerable<RolePermission> aRolePermissions) {
            foreach (RolePermission myRolePermission in aRolePermissions) {
                DeleteRolePermissionWithoutSave(myRolePermission);
            }
        }

        private void DeleteRolePermissionsWithoutSave(Role aRole, IEnumerable<int> aPermissions) {
            foreach (int myPermission in aPermissions) {
                RolePermission myRolePermission = GetRolePermissionByRoleIdAndPermissionId(aRole.Id, myPermission);
                DeleteRolePermissionWithoutSave(myRolePermission);
            }
        }

        private RolePermission GetRolePermissionByRoleIdAndPermissionId(int aRoleId, int aPermissionId) {
            return (from r in theEntities.RolePermissions
                    where r.RoleId == aRoleId
                    && r.PermissionId == aPermissionId
                    select r).FirstOrDefault();
        }

        private void DeleteRolePermissionWithoutSave(RolePermission aRolePermissionToDelete) {
            RolePermission myOriginalRolePermission = GetRolePermission(aRolePermissionToDelete.Id);
            theEntities.DeleteObject(myOriginalRolePermission);
        }

        private void CreateAllRolePermissions(Role aRoleToEdit, IEnumerable<int> aSelectedPermissionIds) {
            if (aSelectedPermissionIds == null) {
                return;
            }

            foreach (int mySelectedPermissionId in aSelectedPermissionIds) {
                CreateRolePermissionWithoutSave(aRoleToEdit.Id, mySelectedPermissionId);
            }
        }

        private void CreateRolePermissionWithoutSave(int aRoleId, int aPermissionId) {
            RolePermission myRolePermissionToAdd = RolePermission.CreateRolePermission(0, aRoleId, aPermissionId);
            theEntities.AddToRolePermissions(myRolePermissionToAdd);
        }
    }
}