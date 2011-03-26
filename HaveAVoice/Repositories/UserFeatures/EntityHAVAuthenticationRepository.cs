using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Services.Helpers;
using Social.User;
using Social.User.Repositories;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthenticationRepository : IHAVAuthenticationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<Permission> FindPermissionsForUser(User aUser) {
            return (from u in theEntities.Users
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    join rp in theEntities.RolePermissions on r.Id equals rp.Role.Id
                    join p in theEntities.Permissions on rp.Permission.Id equals p.Id
                    where u.Id == aUser.Id
                    select p).ToList<Permission>();
        }

        public User FindUserByActivationCode(string anActivationCode) {
            return (from c in theEntities.Users
                    where c.ActivationCode == anActivationCode
                    select c).FirstOrDefault();
        }

        public User FindUserByCookieHash(int aUserId, string aCookieHash) {
            DateTime dateTimeCookieExpires = DateTime.Now.AddHours(CookieHelper.REMEMBER_ME_COOKIE_HOURS);
            return (from c in theEntities.Users
                    where c.Id == aUserId
                    && c.CookieHash == aCookieHash
                    select c).FirstOrDefault();
        }

        public User UpdateCookieHashCreationDate(User aUser) {
            IUserRepository<User, Role, UserRole> myUserRepo = new EntityHAVUserRepository();
            aUser.CookieCreationDate = DateTime.Now;
            UpdateUser(aUser);
            return aUser;
        }

        private void UpdateUser(User aUser) {
            IUserRepository<User, Role, UserRole> myUserRepo = new EntityHAVUserRepository();
            myUserRepo.UpdateUser(aUser);

        }
    }
}