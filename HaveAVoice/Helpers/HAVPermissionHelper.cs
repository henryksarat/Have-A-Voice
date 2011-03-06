using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.UserInformation;
using System.Collections;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers {
    public class HAVPermissionHelper {
        private static Hashtable theRestrictionTable = new Hashtable();

        public static void Reset() {
            theRestrictionTable = new Hashtable();
        }

        public static bool HasPermission(UserInformationModel aUserToCheck, HAVPermission aPermission) {
            return (from p in aUserToCheck.Permissions
                    where p.Name == aPermission.ToString()
                    select p).Count<Permission>() > 0 ? true : false;
        }

        public static bool AllowedToPerformAction(UserInformationModel aUserInformation, params HAVPermission[] aPermissions) {
            bool myResult = false;

            foreach(HAVPermission myPermission in aPermissions) {
                if(AllowedToPerformAction(aUserInformation, myPermission)) {
                    myResult = true;
                    break;
                }
            }

            return myResult;
        }

        public static bool AllowedToPerformAction(UserInformationModel aUserInformation, HAVPermission aPermission) {
            if (aUserInformation == null || !HasPermission(aUserInformation.Permissions, aPermission)) {
                return false;
            }

            List<Pair<RestrictionList, long>> myRestrictions =
                (List<Pair<RestrictionList, long>>)aUserInformation.PermissionToRestriction[aPermission];

            if (myRestrictions == null) {
                return true;
            }

            long myPurgeAfterTimeLimit = Int32.MaxValue;
            foreach (Pair<RestrictionList, long> restriction in myRestrictions) {
                if (restriction.First == RestrictionList.TimeLimit) {
                    myPurgeAfterTimeLimit = restriction.Second;
                    FilterLog(aPermission, myPurgeAfterTimeLimit);
                    break;
                }
            }

            bool myAllowedToPerform = true;
            foreach (Pair<RestrictionList, long> myRestriction in myRestrictions) {
                RestrictionLog myRestrictionLog = (RestrictionLog)theRestrictionTable[aPermission];
                if (myRestrictionLog == null || myRestrictionLog.TimeLog.Count == 0) {
                    break;
                }

                if (myRestriction.First == RestrictionList.SecondsToWait) {
                    DateTime myLatestEntry = myRestrictionLog.TimeLog[myRestrictionLog.TimeLog.Count - 1];
                    DateTime myTimeToContinue = myLatestEntry.AddSeconds(myRestriction.Second);

                    if (myTimeToContinue > DateTime.UtcNow) {
                        myAllowedToPerform = false;
                        break;
                    }

                }
                if (myRestriction.First == RestrictionList.OccurencesWithinTimeLimit) {
                    if (myRestrictionLog.TimeLog.Count >= myRestriction.Second) {
                        myAllowedToPerform = false;
                        break;
                    }
                }
            }

            LogAction(aPermission, myPurgeAfterTimeLimit);

            return myAllowedToPerform;
        }

        private static void LogAction(HAVPermission aPermission, long aTimeLimit) {

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

        private static void FilterLog(HAVPermission aPermission, long aTimeLimit) {
            RestrictionLog restrictionLog;
            if (theRestrictionTable.Contains(aPermission)) {
                restrictionLog = (RestrictionLog)theRestrictionTable[aPermission];
                restrictionLog.FilterLog(aTimeLimit);
            }
        }

        private static bool HasPermission(IEnumerable<Permission> aPermissions, HAVPermission aCheckingPermission) {
            return (from p in aPermissions
                    where p.Name == aCheckingPermission.ToString()
                    select p).Count<Permission>() > 0 ? true : false;
        }
    }
}