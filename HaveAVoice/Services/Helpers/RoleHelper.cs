using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using Social.Generic.Models;
using Social.Generic.Helpers;
using Social.Admin.Helpers;

namespace HaveAVoice.Services.Helpers {
    public class RoleHelper {
        public static bool IsPolitician(UserInformationModel<User> aUser) {
            return PermissionHelper<User>.HasPermission(aUser, SocialPermission.Confirmed_Politician);
        }

        public static bool IsPoliticalCandidate(UserInformationModel<User> aUser) {
            return PermissionHelper<User>.HasPermission(aUser, SocialPermission.Confirmed_Political_Candidate);
        }

        public static bool IsPolitician(User aUserToCheckAgainst) {
            return RolesToCheckAgainst(UserRoleHelper.PoliticianRoles(), aUserToCheckAgainst);
        }

        public static bool IsPoliticalCandidate(User aUserToCheckAgainst) {
            return RolesToCheckAgainst(UserRoleHelper.PoliticalCandidateRoles(), aUserToCheckAgainst);
        }

        private static bool RolesToCheckAgainst(IEnumerable<string> aRoles, User aUserToCheckAgainst) {
            bool myIsPolitician = false;

            foreach (UserRole myUserRole in aUserToCheckAgainst.UserRoles) {
                myIsPolitician = aRoles.Contains(myUserRole.Role.Name);
                if (myIsPolitician) {
                    break;
                }
            }

            return myIsPolitician;
        }
    }
}