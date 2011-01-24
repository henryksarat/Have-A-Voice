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
        public IEnumerable<PhotoAlbumFeedModel> PhotoAlbumFeed { 
            set {
                PhotoAlbumEnumerator = value.GetEnumerator();
                PhotoAlbumEnumerator.MoveNext();
            }
        }

        private IEnumerator<BoardFeedModel> BoardFeedEnumerator { get; set; }
        private IEnumerator<IssueFeedModel> IssueFeedEnumerator { get; set; }
        private IEnumerator<IssueReplyFeedModel> IssueReplyFeedEnumerator { get; set; }
        private IEnumerator<PhotoAlbumFeedModel> PhotoAlbumEnumerator { get; set; }

        public UserProfileModel(User aUser) {
            User = aUser;
            BoardFeed = new List<BoardFeedModel>();
            IssueFeed = new List<IssueFeedModel>();
            IssueReplyFeed = new List<IssueReplyFeedModel>();
            PhotoAlbumFeed = new List<PhotoAlbumFeedModel>();
        }

        public FeedItem GetNextItem() {
            FeedItem myFeedItem = FeedItem.None;
            List<SortableItemMetaData> myCommonSortableList = new List<SortableItemMetaData>();

            if (!IsEmpty()) {
                if (BoardFeedEnumerator.Current != null) {
                    myCommonSortableList.Add(CreateSortable(BoardFeedEnumerator.Current, FeedItem.Board));
                } 
                if (IssueFeedEnumerator.Current != null) {
                    myCommonSortableList.Add(CreateSortable(IssueFeedEnumerator.Current, FeedItem.Issue));
                }
                if (IssueReplyFeedEnumerator.Current != null) {
                    myCommonSortableList.Add(CreateSortable(IssueReplyFeedEnumerator.Current, FeedItem.IssueReply));
                }
                if (PhotoAlbumEnumerator.Current != null) {
                    myCommonSortableList.Add(CreateSortable(PhotoAlbumEnumerator.Current, FeedItem.Photo));
                }

                myFeedItem = myCommonSortableList
                    .OrderBy(s => s.DateTimeStamp)
                    .FirstOrDefault<SortableItemMetaData>()
                    .FeedItem;
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

        public PhotoAlbumFeedModel GetNextPhotoAlbum() {
            PhotoAlbumFeedModel myModel = PhotoAlbumEnumerator.Current;
            PhotoAlbumEnumerator.MoveNext();
            return myModel;
        }

        public bool IsEmpty() {
            return BoardFeedEnumerator.Current == null && IssueFeedEnumerator.Current == null && IssueReplyFeedEnumerator.Current == null && PhotoAlbumEnumerator.Current == null;
        }

        private SortableItemMetaData CreateSortable(FeedModel aFeedModel, FeedItem aFeedItem) {
            return new SortableItemMetaData() {
                DateTimeStamp = aFeedModel.DateTimeStamp,
                FeedItem = aFeedItem
            };
        }

        private class SortableItemMetaData {
            public DateTime DateTimeStamp { get; set; }
            public FeedItem FeedItem { get; set; }
        }
    }
}