using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthenticationRepository : HAVBaseRepository, IHAVAuthenticationRepository {
        public IEnumerable<Permission> FindPermissionsForUser(User aUser) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    join rp in GetEntities().RolePermissions on r.Id equals rp.Role.Id
                    join p in GetEntities().Permissions on rp.Permission.Id equals p.Id
                    where u.Id == aUser.Id
                    select p).ToList<Permission>();
        }

        public User FindUserByActivationCode(string anActivationCode) {
            return (from c in GetEntities().Users
                    where c.ActivationCode == anActivationCode
                    select c).FirstOrDefault();
        }

        public Restriction FindRestrictionsForUser(User aUser) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    join res in GetEntities().Restrictions on r.Restriction.Id equals res.Id
                    where u.Id == aUser.Id
                    select res).FirstOrDefault<Restriction>();
        }

        public User FindUserByCookieHash(int aUserId, string aCookieHash) {
            DateTime dateTimeCookieExpires = DateTime.Now.AddHours(HAVAuthenticationService.REMEMBER_ME_COOKIE_HOURS);
            return (from c in GetEntities().Users
                    where c.Id == aUserId
                    && c.CookieHash == aCookieHash
                    && c.CookieCreationDate >= dateTimeCookieExpires
                    select c).FirstOrDefault();
        }

        public User UpdateCookieHashCreationDate(User aUser) {
            IHAVUserRepository myUserRepo = new EntityHAVUserRepository();
            aUser.CookieCreationDate = DateTime.Now;
            UpdateUser(aUser);
            return aUser;
        }

        private void UpdateUser(User aUser) {
            IHAVUserRepository myUserRepo = new EntityHAVUserRepository();
            myUserRepo.UpdateUser(aUser);

        }
    }
}