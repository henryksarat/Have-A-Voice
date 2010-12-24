using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class ProfileModel {
        public User User { get; set; }
        public IEnumerable<IssueReply> IssueReplys { get; set; }
        public string BoardMessage { get; set; }
        public IEnumerable<Board> BoardMessages { get; set; }
        public FanStatus FanStatus { get; set; }
        public IEnumerable<Fan> Fans { get; set; }
        public IEnumerable<Fan> FansOf { get; set; }

        public ProfileModel(User aUser) {
            User = aUser;
            IssueReplys = new List<IssueReply>();
            BoardMessage = string.Empty;
            BoardMessages = new List<Board>();
            FanStatus = FanStatus.None;
            Fans = new List<Fan>();
            FansOf = new List<Fan>();
        }
    }
}