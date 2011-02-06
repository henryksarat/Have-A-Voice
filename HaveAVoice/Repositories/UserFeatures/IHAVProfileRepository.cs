﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVProfileRepository {
        IEnumerable<Issue> IssuesUserCreated(User aUser);
        IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser);

        IEnumerable<Issue> FriendIssueFeed(User aUser);
        IEnumerable<IssueReply> FriendIssueReplyFeed(User aUser);
        IEnumerable<PhotoAlbum> FriendPhotoAlbumFeed(User aFriendUser);
        IEnumerable<Issue> IssueFeedByRole(IEnumerable<string> aRoles);
        IEnumerable<IssueReply> IssueReplyFeedByRole(IEnumerable<string> aRoles);

        IEnumerable<Issue> UserIssueFeed(int aTargetUserId);
        IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId);

        IEnumerable<Issue> FilteredIssuesFeed(User aUser);
        IEnumerable<IssueReply> FilteredIssueReplysFeed(User aUser);

        Issue RandomLocalIssue(User aForUser);
    }
}