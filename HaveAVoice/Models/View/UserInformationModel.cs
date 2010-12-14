using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.Collections;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;


namespace HaveAVoice.Models.View {
    public class UserInformationModel {
        public User Details { get; set; }
        public List<Permission> Permissions;
        public Hashtable PermissionToRestriction { get; private set; }
        public Hashtable PrivacySettings { get; private set; }

        public UserInformationModel(User aUser, IEnumerable<Permission> aPermissions, Restriction aRestriction, UserPrivacySetting aPrivacySettings) {
            this.Details = aUser;
            this.Permissions = aPermissions.ToList<Permission>();
            AddRestrictionsToHashTable(aRestriction);


            AddPrivacySettingsToHashTable(aPrivacySettings);
        }



        private void AddRestrictionsToHashTable(Restriction aRestriction) {
            PermissionToRestriction = new Hashtable();

            List<Pair<RestrictionList, long>> restrictions = new List<Pair<RestrictionList, long>>();
            restrictions.Add(CreateRestrictionList(RestrictionList.SecondsToWait, aRestriction.IssuePostsWaitTimeBeforePostSeconds));
            restrictions.Add(CreateRestrictionList(RestrictionList.TimeLimit, aRestriction.IssuePostsTimeLimit));
            restrictions.Add(CreateRestrictionList(RestrictionList.OccurencesWithinTimeLimit, aRestriction.IssuePostsWithinTimeLimit));

            PermissionToRestriction.Add(HaveAVoice.Helpers.HAVPermission.Post_Issue, restrictions);

            restrictions = new List<Pair<RestrictionList, long>>();
            restrictions.Add(CreateRestrictionList(RestrictionList.SecondsToWait, aRestriction.IssueReplyPostsWaitTimeBeforePostSeconds));
            restrictions.Add(CreateRestrictionList(RestrictionList.TimeLimit, aRestriction.IssueReplyPostsTimeLimit));
            restrictions.Add(CreateRestrictionList(RestrictionList.OccurencesWithinTimeLimit, aRestriction.IssueReplyPostsWithinTimeLimit));

            PermissionToRestriction.Add(HaveAVoice.Helpers.HAVPermission.Post_Issue_Reply, restrictions);
        }

        private void AddPrivacySettingsToHashTable(UserPrivacySetting aPrivacySettings) {
            PrivacySettings = new Hashtable();
            PrivacySettings.Add(PrivacySetting.DisplayProfileToNonLoggedIn, aPrivacySettings.DisplayProfileToNonLoggedIn);
            PrivacySettings.Add(PrivacySetting.DisplayProfileToFriends, aPrivacySettings.DisplayProfileToFriends);
            PrivacySettings.Add(PrivacySetting.DisplayProfileToLoggedInUsers, aPrivacySettings.DisplayProfileToLoggedInUsers);
        }
        
        private Pair<RestrictionList, long> CreateRestrictionList(RestrictionList restriction, long restrictionValue) {
            Pair<RestrictionList, long> pair = new Pair<RestrictionList, long>();
            pair.First = restriction;
            pair.Second = restrictionValue;
            return pair;
        }
    }
}
