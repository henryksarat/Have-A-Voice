using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class ProfileModel {
        public User User { get; set; }
        public string ProfilePictureUrl { get; set; }
        public IEnumerable<IssueReply> IssueReplys { get; set; }
        public string BoardMessage { get; set; }
        public IEnumerable<Board> BoardMessages { get; set; }
        public FriendStatus FriendStatus { get; set; }
        public IEnumerable<Friend> Friends { get; set; }

        public ProfileModel(User aUser) {
            User = aUser;
            ProfilePictureUrl = HAVConstants.NO_PROFILE_PICTURE_URL;
            IssueReplys = new List<IssueReply>();
            BoardMessage = string.Empty;
            BoardMessages = new List<Board>();
            FriendStatus = FriendStatus.None;
            Friends = new List<Friend>();
        }
    }
}