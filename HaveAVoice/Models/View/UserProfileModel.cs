using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class UserProfileModel {
        public User User { get; set; }
        public IEnumerable<BoardFeedModel> BoardFeed { get; set; }
        public IEnumerable<IssueFeedModel> IssueFeed { get; set; }
        public IEnumerable<IssueReplyFeedModel> IssueReplyFeed { get; set; }

        public UserProfileModel(User aUser) {
            User = aUser;
            BoardFeed = new List<BoardFeedModel>();
            IssueFeed = new List<IssueFeedModel>();
            IssueReplyFeed = new List<IssueReplyFeedModel>();
        }

        public FeedItem GetNextItem() {
            return FeedItem.Issue;
        }

        public IssueFeedModel GetNextIssue() {
            return IssueFeed.FirstOrDefault();
        }

        public BoardFeedModel GetNextBoard() {
            return BoardFeed.FirstOrDefault();
        }

        public IssueReplyFeedModel GetNextIssueReply() {
            return IssueReplyFeed.FirstOrDefault();
        }
    }
}