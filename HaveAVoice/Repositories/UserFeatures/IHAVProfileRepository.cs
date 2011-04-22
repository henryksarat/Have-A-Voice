using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVProfileRepository {
        IEnumerable<Issue> AuthorityIssuesFeedByCity(User aUser);
        IEnumerable<Issue> AuthorityIssuesFeedByState(User aUser);
        IEnumerable<Issue> AuthorityIssuesFeedByZipCode(User aUser);
        IEnumerable<IssueReply> AuthorityIssueReplysFeedByCity(User aUser);
        IEnumerable<IssueReply> AuthorityIssueReplysFeedByState(User aUser);
        IEnumerable<IssueReply> AuthorityIssueReplysFeedByZipCode(User aUser);


        IEnumerable<Issue> IssuesUserCreated(User aUser);
        IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser);

        IEnumerable<Issue> FriendIssueFeed(User aUser);
        IEnumerable<IssueReply> FriendIssueReplyFeed(User aUser);
        IEnumerable<PhotoAlbum> FriendPhotoAlbumFeed(User aFriendUser);
        IEnumerable<Issue> IssueFeedByRole(IEnumerable<string> aRoles);
        IEnumerable<IssueReply> IssueReplyFeedByRole(IEnumerable<string> aRoles);

        IEnumerable<Issue> UserIssueFeed(int aTargetUserId);
        IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId);

        Issue RandomLocalIssue(User aForUser);
    }
}