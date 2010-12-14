using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Models.View {
    public class ProfileModel {
        public User User { get; private set; }
        public IEnumerable<IssueReply> IssueReplys { get; private set; }
        public string BoardMessage { get; private set; }
        public IEnumerable<Board> BoardMessages { get; private set; }
        public bool IsFan { get; private set; }
        public IEnumerable<Fan> Fans { get; private set; }
        public IEnumerable<Fan> FansOf { get; private set; }

        public ProfileModel(ProfileModelBuilder aBuilder) {
            User = aBuilder.GetUser();
            IssueReplys = aBuilder.GetIssueReplys();
            BoardMessage = aBuilder.GetBoardMessage();
            BoardMessages = aBuilder.GetBoardMessages();
            IsFan = aBuilder.GetIsFan();
            Fans = aBuilder.GetFans();
            FansOf = aBuilder.GetFansOf();
        }
    }
}