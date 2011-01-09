using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : IHAVProfileRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser) {
            return (from ir in theEntities.IssueReplys
                    where ir.User.Id == aUser.Id
                    select ir)
                    .OrderByDescending(ir => ir.DateTimeStamp)
                    .Take(5)
                    .ToList<IssueReply>();
        }

        public IEnumerable<Issue> FanIssueFeed(User aUser) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.UserId equals u.Id
                    join f in theEntities.Fans on u.Id equals f.SourceUserId
                    where u.Id != aUser.Id
                    && (f.FanUserId == aUser.Id || f.SourceUserId == aUser.Id)
                    && f.Approved == true
                    && i.Deleted == false
                    select i).OrderByDescending(ir => ir.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> FanIssueReplyFeed(User aUser) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join f in theEntities.Fans on u.Id equals f.SourceUser.Id
                    where (f.FanUserId == aUser.Id || f.SourceUserId == aUser.Id)
                    && u.Id != aUser.Id
                    && f.Approved == true
                    && ir.Deleted == false
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<Issue> UserIssueFeed(int aTargetUserId) {
            return (from f in theEntities.Issues
                    where f.UserId == aTargetUserId
                    select f).ToList<Issue>();
        }

        public IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId) {
            return (from f in theEntities.IssueReplys
                    where f.UserId == aTargetUserId
                    select f).ToList<IssueReply>();
        }
    }
}