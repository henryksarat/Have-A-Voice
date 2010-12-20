using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthenticationRepository : HAVBaseRepository, IHAVAuthenticationRepository {
        public IEnumerable<Permission> GetPermissionsForUser(User aUser) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    join rp in GetEntities().RolePermissions on r.Id equals rp.Role.Id
                    join p in GetEntities().Permissions on rp.Permission.Id equals p.Id
                    where u.Id == aUser.Id
                    select p).ToList<Permission>();
        }
    }
}