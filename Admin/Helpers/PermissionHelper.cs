using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Generic;
using Social.Generic.Helpers;
using Social.Generic.Models;

namespace Social.Admin.Helpers {
    public class PermissionHelper<T> {
        private static Hashtable theRestrictionTable = new Hashtable();

        public static void Reset() {
            theRestrictionTable = new Hashtable();
        }
        
        public static bool HasPermission(UserInformationModel<T> aUserToCheck, SocialPermission aPermission) {
            return aUserToCheck.Permissions.Contains(aPermission);
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

            List<Pair<SocialRestriction, long>> myRestrictions =
                (List<Pair<SocialRestriction, long>>)aUserInformation.PermissionToRestriction[aPermission];

            if (myRestrictions == null) {
                return true;
            }

            long myPurgeAfterTimeLimit = Int32.MaxValue;
            foreach (Pair<SocialRestriction, long> restriction in myRestrictions) {
                if (restriction.First == SocialRestriction.TimeLimit) {
                    myPurgeAfterTimeLimit = restriction.Second;
                    FilterLog(aPermission, myPurgeAfterTimeLimit);
                    break;
                }
            }

            bool myAllowedToPerform = true;
            foreach (Pair<SocialRestriction, long> myRestriction in myRestrictions) {
                RestrictionLog myRestrictionLog = (RestrictionLog)theRestrictionTable[aPermission];
                if (myRestrictionLog == null || myRestrictionLog.TimeLog.Count == 0) {
                    break;
                }

                if (myRestriction.First == SocialRestriction.SecondsToWait) {
                    DateTime myLatestEntry = myRestrictionLog.TimeLog[myRestrictionLog.TimeLog.Count - 1];
                    DateTime myTimeToContinue = myLatestEntry.AddSeconds(myRestriction.Second);

                    if (myTimeToContinue > DateTime.UtcNow) {
                        myAllowedToPerform = false;
                        break;
                    }

                }
                if (myRestriction.First == SocialRestriction.OccurencesWithinTimeLimit) {
                    if (myRestrictionLog.TimeLog.Count >= myRestriction.Second) {
                        myAllowedToPerform = false;
                        break;
                    }
                }
            }

            LogAction(aPermission, myPurgeAfterTimeLimit);

            return myAllowedToPerform;
        }

        private static void LogAction(SocialPermission aPermission, long aTimeLimit) {

            RestrictionLog restrictionLog;
            if (theRestrictionTable.Contains(aPermission)) {
                restrictionLog = (RestrictionLog)theRestrictionTable[aPermission];
                theRestrictionTable.Remove(aPermission);
            } else {
                restrictionLog = RestrictionLog.Create();

            }
            restrictionLog.AddLog(DateTime.UtcNow, aTimeLimit);
            theRestrictionTable.Add(aPermission, restrictionLog);
            RestrictionLog myRestrictionLog = (RestrictionLog)theRestrictionTable[aPermission];

        }

        private static void FilterLog(SocialPermission aPermission, long aTimeLimit) {
            RestrictionLog restrictionLog;
            if (theRestrictionTable.Contains(aPermission)) {
                restrictionLog = (RestrictionLog)theRestrictionTable[aPermission];
                restrictionLog.FilterLog(aTimeLimit);
            }
        }
    }
}
