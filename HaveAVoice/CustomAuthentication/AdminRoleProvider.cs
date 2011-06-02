using System;
using System.Web.Security;
using Social.Generic.Helpers;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using Social.Admin.Services;
using Social.Admin.Repositories;
using Social.Admin;
using HaveAVoice.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Repositories.AdminFeatures;

namespace HaveAVoice.CustomAuthentication {
    public class AdminRoleProvider : RoleProvider {
        IPermissionRepository<User, Permission> thePermissionRepo;

        public AdminRoleProvider()
            : this(new EntityHAVPermissionRepository()) {
        }

        public AdminRoleProvider(IPermissionRepository<User, Permission> aRepository)
            : base() {
                thePermissionRepo = aRepository ?? new EntityHAVPermissionRepository();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override string ApplicationName {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles() {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username) {
            int myUserId;
            int.TryParse(username, out myUserId);
            IEnumerable<Permission> myPermissions = thePermissionRepo.GetPermissionsForUserId(myUserId);

            string[] myRoles = new string[myPermissions.Count<Permission>()];

            myRoles = myPermissions.Select(p => p.Name).ToArray();

            return myRoles;
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName) {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName) {
            throw new NotImplementedException();
        }
    }
}