using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using Social.User.Helpers;
using HaveAVoice.Models.SocialWrappers;

namespace HaveAVoice.Helpers {
    public class NameHelper {
        public static string IssueUrl(Issue anIssue) {
            return anIssue.Title.Replace(' ', '-');
        }

        public static string FullName(User aUser) {
            if (aUser.UseUsername) {
                return aUser.Username;
            } else {
                return NameHelper<User>.FullName(SocialUserModel.Create(aUser));
            }
        }
    }
}