using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.Helpers {
    public class RoleHelper {
        public static bool IsPolitician(UserInformationModel aUser) {
            IEnumerable<string> myPermissions = (from p in aUser.Permissions
                                                 select p.Name).ToList<string>();

            return myPermissions.Contains(HAVPermission.Verified_Political_Candidate.ToString());
        }
    }
}