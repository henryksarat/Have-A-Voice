using System;
using System.Collections.Generic;
using System.Linq;
using Social.Authentication.Repositories;
using Social.Generic.Models;
using Social.User;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Repositories.AuthenticationRepos {
    public class EntityAuthenticationRepository : IAuthenticationRepository<User, Permission> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<AbstractPermissionModel<Permission>> FindPermissionsForUser(User aUser) {
            IEnumerable<Permission> myPermissions = 
                   (from u in theEntities.Users
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    join rp in theEntities.RolePermissions on r.Id equals rp.Role.Id
                    join p in theEntities.Permissions on rp.Permission.Id equals p.Id
                    where u.Id == aUser.Id
                    select p).ToList<Permission>();

            return myPermissions.Select(p => SocialPermissionModel.Create(p));
        }

        public User FindUserByActivationCode(string anActivationCode) {
            return (from c in theEntities.Users
                    where c.ActivationCode == anActivationCode
                    select c).FirstOrDefault();
        }

        public AbstractUserModel<User> GetAbstractUserByActivationCode(string anActivationCode) {
            return SocialUserModel.Create(FindUserByActivationCode(anActivationCode));
        }

        public User FindUserByCookieHash(int aUserId, string aCookieHash) {
            return (from c in theEntities.Users
                    where c.Id == aUserId
                    && c.CookieHash == aCookieHash
                    select c).FirstOrDefault();
        }

        public User UpdateCookieHashCreationDate(User aUser) {
            IUserRepository<User, Role, UserRole> myUserRepo = new EntityUserRepository();
            aUser.CookieCreationDate = DateTime.Now;
            UpdateUser(aUser);
            return aUser;
        }

        private void UpdateUser(User aUser) {
            IUserRepository<User, Role, UserRole> myUserRepo = new EntityUserRepository();
            myUserRepo.UpdateUser(aUser);

        }
    }
}