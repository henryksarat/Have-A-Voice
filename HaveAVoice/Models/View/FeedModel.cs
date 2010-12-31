using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class FeedModel {
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }
        public IssueType IssueType { get; set; }
        public DateTime DateTimeStamp { get; set;  }
        public string Title { get; set; }
        public string Body { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool HasDisposition { get; set; }
        public int TotalReplys { get; set; }

        public FeedModel() {
            Username = string.Empty;
            ProfilePictureUrl = HAVConstants.NO_PROFILE_PICTURE_URL;
            IssueType = IssueType.Issue;
            Title = string.Empty;
            Body = string.Empty;
        }
    }
}