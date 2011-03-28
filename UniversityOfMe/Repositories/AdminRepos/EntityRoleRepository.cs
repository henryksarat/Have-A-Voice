using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Admin.Repositories;
using Social.Generic.Models;
using Social.User;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Repositories.AdminRepos {
    public class EntityRoleRepository : IRoleRepository<User, Role, RolePermission> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Role Create(User aCreatedByUser, Role aRoleToCreate, IEnumerable<int> aPermissionId) {
            throw new NotImplementedException();
        }

        public Role Edit(User aEditedByUser, Role aRoleToEdit, IEnumerable<int> aPermissionsToCreateAssociationWith, IEnumerable<int> aPermissionsToDeleteAssociationWith) {
            throw new NotImplementedException();
        }

        public void Delete(User aDeletedByUser, Role aRoleToDelete) {
            throw new NotImplementedException();
        }

        public Role FindRole(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindUsersInRole(int aRoleId) {
            throw new NotImplementedException();
        }

        public AbstractRoleModel<Role> GetAbstractDefaultRole() {
            throw new NotImplementedException();
        }

        public AbstractRoleModel<Role> GetAbstractRoleByName(string aName) {
            throw new NotImplementedException();
        }

        public IEnumerable<AbstractRolePermissionModel<RolePermission>> GetAbstractRolePermissionsByRole(Role aRoleToGetPermissionsFor) {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAllRoles() {
            throw new NotImplementedException();
        }

        public Role GetRoleByName(string aName) {
            throw new NotImplementedException();
        }

        public void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            throw new NotImplementedException();
        }
    }
}