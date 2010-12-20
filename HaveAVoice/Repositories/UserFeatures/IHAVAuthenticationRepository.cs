using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthenticationRepository {
        IEnumerable<Permission> GetPermissionsForUser(User aUser);
    }
}