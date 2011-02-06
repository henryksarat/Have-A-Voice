using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Helpers {
    public static class UserRoleHelper {
        public static IEnumerable<string> PoliticianRoles() {
            List<string> myOfficialRoles = new List<string>();
            myOfficialRoles.Add(Roles.POLITICIAN);
            return myOfficialRoles;
        }

        public static IEnumerable<string> PoliticalCandidateRoles() {
            List<string> myOfficialRoles = new List<string>();
            myOfficialRoles.Add(Roles.POLITICAL_CANDIDATE);
            return myOfficialRoles;
        }

        public static IEnumerable<string> RegisteredRoles() {
            List<string> myRegisteredRoles = new List<string>();
            myRegisteredRoles.Add(Roles.ADMIN);
            myRegisteredRoles.Add(Roles.REGISTERED);
            return myRegisteredRoles;
        }
    }
}