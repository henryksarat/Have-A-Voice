using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInModel {
        public User User { get; private set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<IssueReply> FanIssueReplys { get; set; }
        public IEnumerable<IssueReply> OfficialsReplys { get; set; }
        public int NewMessageCount { get; set; }
        public int PendingFanRequestsCount { get; set; }
        public int NotificatonCount { get; set; }

        public LoggedInModel(User aUser) {
            User = aUser;
            ProfilePictureURL = HAVConstants.NO_PROFILE_PICTURE_URL;
            FanIssueReplys = new List<IssueReply>();
            OfficialsReplys = new List<IssueReply>();
        }
    }
}