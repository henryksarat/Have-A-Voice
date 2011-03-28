using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Admin.Repositories;
using UniversityOfMe.Models;
using Social.Generic.Constants;

namespace UniversityOfMe.Repositories.AdminRepos {
    public class EntityRoleRepository : IRoleRepository<User, Role> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Role Create(User aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds) {
            throw new NotImplementedException();
        }

        public Role Edit(User aEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissionIds) {
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

        public IEnumerable<Role> GetAllRoles() {
            throw new NotImplementedException();
        }

        public Role GetDefaultRole() {
            throw new NotImplementedException();
        }

        public Role GetRoleByName(string aName) {
            throw new NotImplementedException();
        }

        public Role GetNotConfirmedUserRole() {
            Role notConfirmedRole = (from c in theEntities.Roles
                                     where c.Name == Constants.NOT_CONFIRMED_USER_ROLE
                                     select c).FirstOrDefault();

            if (notConfirmedRole == null)
                throw new NullReferenceException("Unable to get the Not Confirmed User Role.");

            return notConfirmedRole;
        }

        public void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            throw new NotImplementedException();
        }


        public Social.Generic.Models.AbstractRoleModel<Role> GetAbstractNotConfirmedUserRole() {
            throw new NotImplementedException();
        }

        public Social.Generic.Models.AbstractRoleModel<Role> GetAbstractDefaultRole() {
            throw new NotImplementedException();
        }

        public Social.Generic.Models.AbstractRoleModel<Role> GetAbstractRoleByName(string aRoleName) {
            throw new NotImplementedException();
        }
    }
}