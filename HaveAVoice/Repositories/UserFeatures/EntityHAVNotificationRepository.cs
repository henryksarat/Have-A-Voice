using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.Groups;
using HaveAVoice.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVNotificationRepository : IHAVNotificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<GroupMember> UnapprovedGroupMembers(User aUser) {
            IEnumerable<int> myGroupsAdminOf = GetGroupsAdminOf(aUser);

            return (from gm in theEntities.GroupMembers
                    where !gm.OldRecord
                    && gm.Approved == HAVConstants.PENDING
                    && !gm.AutoAccepted
                    && gm.DeniedByUser == null
                    && gm.ApprovedByUser == null
                    && myGroupsAdminOf.Contains(gm.GroupId)
                    select gm);

        }

        public IEnumerable<Board> UnreadBoardMessages(User aUser) {
            return (from b in theEntities.Boards
                    join v in theEntities.BoardViewedStates on b.Id equals v.BoardId
                    where v.Viewed == false
                    && b.OwnerUserId == aUser.Id
                    && v.UserId == aUser.Id
                    select b).ToList<Board>();
        }

        public IEnumerable<BoardViewedState> UnreadParticipatingBoardMessages(User aUser) {
            return (from v in theEntities.BoardViewedStates
                    join b in theEntities.Boards on v.BoardId equals b.Id
                    where v.Viewed == false
                    && v.UserId == aUser.Id
                    && b.OwnerUserId != aUser.Id
                    select v).ToList<BoardViewedState>();
        }

        public IEnumerable<IssueViewedState> UnreadIssues(User aUser) {
            return (from v in theEntities.IssueViewedStates
                    where v.UserId == aUser.Id
                    && v.Viewed == false
                    select v).ToList<IssueViewedState>();
        }

        public IEnumerable<IssueReplyViewedState> UnreadIssueReplies(User aUser) {
            return (from v in theEntities.IssueReplyViewedStates
                    join ir in theEntities.IssueReplys on v.IssueReplyId equals ir.Id
                    where v.Viewed == false
                    && v.UserId == aUser.Id
                    && ir.UserId == aUser.Id
                    select v).ToList<IssueReplyViewedState>();
        }

        public IEnumerable<IssueReplyViewedState> UnreadParticipatingIssueReplies(User aUser) {
            return (from v in theEntities.IssueReplyViewedStates
                    join ir in theEntities.IssueReplys on v.IssueReplyId equals ir.Id
                    where v.Viewed == false
                    && v.UserId == aUser.Id
                    && ir.UserId != aUser.Id
                    select v).ToList<IssueReplyViewedState>();
        }

        public IEnumerable<GroupMember> UnreadGroupBoardPosts(User aUser) {
            return (from gm in theEntities.GroupMembers
                    where gm.MemberUserId == aUser.Id
                    && !gm.Deleted
                    && !gm.OldRecord
                    && !gm.BoardViewed
                    select gm);
        }

        private IEnumerable<int> GetGroupsAdminOf(User aUser) {
            return (from cm in theEntities.GroupMembers
                    where cm.MemberUserId == aUser.Id
                    && cm.Administrator
                    select cm.GroupId);
        }
    }
}