using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : IHAVProfileRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<Issue> AuthorityIssuesFeedByCity(User aUser) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && i.Deleted == false
                    && i.City == aUser.City
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<Issue> AuthorityIssuesFeedByState(User aUser) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && i.Deleted == false
                    && i.State == aUser.State
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<Issue> AuthorityIssuesFeedByZipCode(User aUser) {
            IEnumerable<int> myAuthoritiesViewableZipCodes = GetAuthorityViewableZipCodes(aUser);

            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && i.Deleted == false
                    && myAuthoritiesViewableZipCodes.Contains(i.Zip)
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> AuthorityIssueReplysFeedByCity(User aUser) {
            IEnumerable<int> myAuthoritiesViewableZipCodes = GetAuthorityViewableZipCodes(aUser);

            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && ir.Deleted == false
                    && ir.City == aUser.City
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<IssueReply> AuthorityIssueReplysFeedByState(User aUser) {
            IEnumerable<int> myAuthoritiesViewableZipCodes = GetAuthorityViewableZipCodes(aUser);

            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && ir.Deleted == false
                    && ir.State == aUser.State
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<IssueReply> AuthorityIssueReplysFeedByZipCode(User aUser) {
            IEnumerable<int> myAuthoritiesViewableZipCodes = GetAuthorityViewableZipCodes(aUser);

            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && ir.Deleted == false
                    && myAuthoritiesViewableZipCodes.Contains(ir.Zip)
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

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
                    select i).OrderByDescending(i2 => i2.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> IssueReplyFeedByRole(IEnumerable<string> aRoles) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where aRoles.Contains(r.Name)
                    && ir.Deleted == false
                    select ir).OrderByDescending(i2 => i2.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<Issue> UserIssueFeed(int aTargetUserId) {
            return (from f in theEntities.Issues
                    where f.UserId == aTargetUserId
                    && f.Deleted == false
                    select f).OrderByDescending(i2 => i2.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId) {
            return (from f in theEntities.IssueReplys
                    where f.UserId == aTargetUserId
                    && f.Deleted == false
                    select f).OrderByDescending(f2 => f2.DateTimeStamp).ToList<IssueReply>();
        }
        
        public Issue RandomLocalIssue(User aForUser) {
            IEnumerable<Issue> myLocalIssues = (from i in theEntities.Issues
                                                where i.City == aForUser.City
                                                && i.State == aForUser.State
                                                select i);

            int myCount = myLocalIssues.Count<Issue>();
            if (myCount == 0) {
                myLocalIssues = (from i in theEntities.Issues
                                            where i.State == aForUser.State
                                            select i);
                myCount = myLocalIssues.Count<Issue>();

                if (myCount == 0) {
                    myLocalIssues = (from i in theEntities.Issues
                                                select i);

                    myCount = myLocalIssues.Count<Issue>();
                }
            }

            int myRandomIndex = new Random().Next(myCount);

            return myLocalIssues.Skip(myRandomIndex).FirstOrDefault<Issue>();
        }

        private IEnumerable<int> GetAuthorityViewableZipCodes(User aUser) {
            return (from z in theEntities.AuthorityViewableZipCodes
                    where z.UserId == aUser.Id
                    select z.ZipCode).ToList<int>();
        }
    }
}