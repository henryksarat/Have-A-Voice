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

            return myPermissions.Contains(HAVPermission.Confirmed_Politician.ToString());
        }

        public static bool IsPoliticalCandidate(UserInformationModel aUser) {
            IEnumerable<string> myPermissions = (from p in aUser.Permissions
                                                 select p.Name).ToList<string>();
            return myPermissions.Contains(HAVPermission.Confirmed_Political_Candidate.ToString());
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