using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers {
    public class NameHelper {
        public static string IssueUrl(Issue anIssue) {
            return anIssue.Title.Replace(' ', '-');
        }

        public static string FullName(User aUser) {
            return aUser.FirstName + " " + aUser.LastName;
        }
    }
}