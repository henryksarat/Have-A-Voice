using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class UserProfileModel {
        public User User { get; set; }
        public IEnumerable<BoardFeedModel> BoardFeed { set {
                BoardFeedEnumerator = value.GetEnumerator();
                BoardFeedEnumerator.MoveNext();
            }
        }
        public IEnumerable<IssueFeedModel> IssueFeed {  
            set {
                IssueFeedEnumerator = value.GetEnumerator();
                IssueFeedEnumerator.MoveNext();
            }
        }
        public IEnumerable<IssueReplyFeedModel> IssueReplyFeed { 
            set {
                IssueReplyFeedEnumerator = value.GetEnumerator();
                IssueReplyFeedEnumerator.MoveNext();
            }
        }

        private IEnumerator<BoardFeedModel> BoardFeedEnumerator { get; set; }
        private IEnumerator<IssueFeedModel> IssueFeedEnumerator { get; set; }
        private IEnumerator<IssueReplyFeedModel> IssueReplyFeedEnumerator { get; set; }

        public UserProfileModel(User aUser) {
            User = aUser;
            BoardFeed = new List<BoardFeedModel>();
            IssueFeed = new List<IssueFeedModel>();
            IssueReplyFeed = new List<IssueReplyFeedModel>();
        }

        public FeedItem GetNextItem() {
            FeedItem myFeedItem = FeedItem.None;
            if (BoardFeedEnumerator.Current != null || IssueFeedEnumerator.Current != null || IssueReplyFeedEnumerator.Current != null) {
                if (BoardFeedEnumerator.Current != null) {
                    myFeedItem = FeedItem.Board;
                } else if (IssueFeedEnumerator.Current != null) {
                    myFeedItem = FeedItem.Issue;
                } else if (IssueReplyFeedEnumerator.Current != null) {
                    myFeedItem = FeedItem.IssueReply;
                }
            }
            
            return myFeedItem;
        }

        public IssueFeedModel GetNextIssue() {
            IssueFeedModel myModel = IssueFeedEnumerator.Current;
            IssueFeedEnumerator.MoveNext();
            return myModel;
        }

        public BoardFeedModel GetNextBoard() {
            BoardFeedModel myModel = BoardFeedEnumerator.Current;
            BoardFeedEnumerator.MoveNext();
            return myModel;
        }

        public IssueReplyFeedModel GetNextIssueReply() {
            IssueReplyFeedModel myModel = IssueReplyFeedEnumerator.Current;
            IssueReplyFeedEnumerator.MoveNext();
            return myModel;
        }
    }
}