﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVNotificationRepository {
        IEnumerable<GroupInvitation> GroupInvitations(User aUser);
        IEnumerable<GroupMember> UnapprovedGroupMembers(User aUser);
        IEnumerable<Board> UnreadBoardMessages(User aUser);
        IEnumerable<BoardViewedState> UnreadParticipatingBoardMessages(User aUser);
        IEnumerable<IssueViewedState> UnreadIssues(User aUser);
        IEnumerable<IssueReplyViewedState> UnreadIssueReplies(User aUser);
        IEnumerable<IssueReplyViewedState> UnreadParticipatingIssueReplies(User aUser);
        IEnumerable<GroupMember> UnreadGroupBoardPosts(User aUser);
    }
}