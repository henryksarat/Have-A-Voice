using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : IHAVProfileRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<Issue> IssuesUserCreated(User aUser) {
            return (from i in theEntities.Issues
                    where i.UserId == aUser.Id
                    && i.Deleted == false
                    select i)
                    .OrderByDescending(i => i.DateTimeStamp)
                    .Take(5)
                    .ToList<Issue>();
        }

        public IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser) {
            return (from ir in theEntities.IssueReplys
                    where ir.User.Id == aUser.Id
                    && ir.Deleted == false
                    select ir)
                    .OrderByDescending(ir => ir.DateTimeStamp)
                    .Take(5)
                    .ToList<IssueReply>();
        }

        public IEnumerable<Issue> FriendIssueFeed(User aUser) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.UserId equals u.Id
                    join f in theEntities.Friends on u.Id equals f.SourceUserId
                    where u.Id != aUser.Id
                    && (f.FriendUserId == aUser.Id || f.SourceUserId == aUser.Id)
                    && f.Approved == true
                    && i.Deleted == false
                    select i).OrderByDescending(ir => ir.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> FriendIssueReplyFeed(User aUser) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join f in theEntities.Friends on u.Id equals f.SourceUserId
                    where (f.FriendUserId == aUser.Id || f.SourceUserId == aUser.Id)
                    && u.Id != aUser.Id
                    && f.Approved == true
                    && ir.Deleted == false
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<PhotoAlbum> FriendPhotoAlbumFeed(User aFriendUser) {
            return (from p in theEntities.PhotoAlbums
                    join u in theEntities.Users on p.User.Id equals u.Id
                    join f in theEntities.Friends on u.Id equals f.SourceUserId
                    where (f.FriendUserId == aFriendUser.Id || f.SourceUserId == aFriendUser.Id)
                    && u.Id != aFriendUser.Id
                    && f.Approved == true
                    select p).ToList<PhotoAlbum>();
        }

        public IEnumerable<Issue> IssueFeedByRole(IEnumerable<string> aRoles) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where aRoles.Contains(r.Name)
                    && i.Deleted == false
                    select i).ToList<Issue>();
        }

        public IEnumerable<IssueReply> IssueReplyFeedByRole(IEnumerable<string> aRoles) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where aRoles.Contains(r.Name)
                    && ir.Deleted == false
                    select ir).ToList<IssueReply>();
        }

        public IEnumerable<Issue> UserIssueFeed(int aTargetUserId) {
            return (from f in theEntities.Issues
                    where f.UserId == aTargetUserId
                    && f.Deleted == false
                    select f).OrderByDescending(f2 => f2.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId) {
            return (from f in theEntities.IssueReplys
                    where f.UserId == aTargetUserId
                    && f.Deleted == false
                    select f).OrderByDescending(f2 => f2.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<Issue> FilteredIssuesFeed(User aUser) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && i.Deleted == false
                    && i.City == u.City
                    && i.State == u.State
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> FilteredIssueReplysFeed(User aUser) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && ir.Deleted == false
                    && ir.City == u.City
                    && ir.State == u.State
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        private IEnumerable<FilteredCityState> FindFilteredCityStateForUser(User aUser) {
            return (from f in theEntities.FilteredCityStates
                    where f.UserId == aUser.Id
                    select f).ToList<FilteredCityState>();
        }

        private IEnumerable<FilteredZipCode> FindFilteredZipCodeForUser(User aUser) {
            return (from f in theEntities.FilteredZipCodes
                    where f.UserId == aUser.Id
                    select f).ToList<FilteredZipCode>();
        }
    }
}