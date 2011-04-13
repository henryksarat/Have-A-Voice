using System.Collections;
using System.Linq;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace Social.Admin.Helpers {
    public class PermissionHelper<T> {
        private static Hashtable theRestrictionTable = new Hashtable();

        public static void Reset() {
            theRestrictionTable = new Hashtable();
        }
        
        public static bool HasPermission(UserInformationModel<T> aUserToCheck, SocialPermission aPermission) {
            return aUserToCheck.Permissions.Contains(aPermission);
        }

        public static bool AllowedToPerformAction(IValidationDictionary aValidationDictionary, UserInformationModel<T> aUser, SocialPermission aPermission) {
            if (!PermissionHelper<T>.AllowedToPerformAction(aUser, aPermission)) {
                aValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return aValidationDictionary.isValid;
        }
        
        public static bool AllowedToPerformAction(UserInformationModel<T> aUserInformation, params SocialPermission[] aPermissions) {
            bool myResult = false;

            foreach (SocialPermission myPermission in aPermissions) {
                if (AllowedToPerformAction(aUserInformation, myPermission)) {
                    myResult = true;
                    break;
                }
            }

            return myResult;
        }

        public static bool AllowedToPerformAction(UserInformationModel<T> aUserInformation, SocialPermission aPermission) {
            if (aUserInformation == null || !HasPermission(aUserInformation, aPermission)) {
                return false;
            }
            return true;
        }
    }
}
