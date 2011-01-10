using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVProfileRepository {
        IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser);

        //IEnumerable<Issue> OfficialsIssueFeed(User aViewingUser, IEnumerable<string> aSelectRoles);
        //IEnumerable<IssueReply> OfficialsIssueReplyFeed(User aViewingUser, IEnumerable<string> aSelectRoles);

        IEnumerable<Issue> FriendIssueFeed(User aUser);
        IEnumerable<IssueReply> FriendIssueReplyFeed(User aUser);

        IEnumerable<Issue> UserIssueFeed(int aTargetUserId);
        IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId);
    }
}