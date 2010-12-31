using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVHomeRepository : IHAVHomeRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        private static double ISSUE_DISPOSITION_WEIGHT = 0.2;
        private static double ISSUE_REPLY_WEIGHT = 0.8;

        public IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition) {
            IEnumerable<Issue> myIssues = theEntities.Issues;
            IEnumerable<IssueDisposition> myIssueDispositions = theEntities.IssueDispositions;
            IEnumerable<IssueReply> myIssueReplys = theEntities.IssueReplys;

            return Helpers.CalculatedWithWeights(myIssues.ToList(), myIssueReplys.ToList(), myIssueDispositions.ToList(), aDisposition);
        }

        public IEnumerable<IssueReply> GetMostPopularIssueReplys() {
            IEnumerable<IssueReply> myReplys = theEntities.IssueReplys;
            IEnumerable<IssueReplyComment> myComments = theEntities.IssueReplyComments;
            IEnumerable<IssueReplyDisposition> myDispositions = theEntities.IssueReplyDispositions;

            return Helpers.CalculatedWithWeights(myReplys.ToList(), myComments.ToList(), myDispositions.ToList());
        }

        public static class Helpers {
            public static IEnumerable<IssueWithDispositionModel> CalculatedWithWeights(List<Issue> anIssues,
                List<IssueReply> anIssueReplys,
                List<IssueDisposition> anIssueDispositions,
                Disposition aDisposition) {

                List<WeightModelIssue> myWeightModel = (from i in anIssues
                                                   let d = anIssueDispositions
                                                   .Where(d2 => d2.Issue.Id == i.Id)
                                                   .Where(d2 => d2.Disposition == (int)aDisposition)
                                                   .ToList<IssueDisposition>()
                                                   let ir = anIssueReplys
                                                   .Where(ir2 => ir2.Deleted == false)
                                                   .Where(ir2 => ir2.Issue.Id == i.Id)
                                                   .Where(ir2 => ir2.Disposition == (int)aDisposition)
                                                   .ToList<IssueReply>()
                                                   where i.Deleted == false 
                                                   && (d.Count > 0 || ir.Count > 0)
                                                   select new WeightModelIssue {
                                                       Issue = i,
                                                       WeightedAverage = (d.Count * ISSUE_DISPOSITION_WEIGHT) + (ir.Count * ISSUE_REPLY_WEIGHT)
                                                   }).OrderByDescending(b2 => b2.WeightedAverage).ToList<WeightModelIssue>();

                return (from b in myWeightModel
                        select new IssueWithDispositionModel {
                            Issue = b.Issue,
                            HasDisposition = true
                        }).ToList<IssueWithDispositionModel>();

            }

            public static IEnumerable<IssueReply> CalculatedWithWeights(List<IssueReply> anIssueReply,
                List<IssueReplyComment> anIssueReplyComments,
                List<IssueReplyDisposition> aDispositions) {

                List<WeightModelReply> myWeightModel = (from ir in anIssueReply
                                                    let d = aDispositions
                                                   .Where(d2 => d2.IssueReply.Id == ir.Id)
                                                   .ToList<IssueReplyDisposition>()
                                                   let c = anIssueReplyComments
                                                   .Where(c2 => c2.IssueReply.Id == ir.Id)
                                                   .ToList<IssueReplyComment>()
                                                   where ir.Deleted == false
                                                   && ir.Issue.Deleted == false
                                                   select new WeightModelReply {
                                                       IssueReply = ir,
                                                       WeightedAverage = (d.Count * ISSUE_DISPOSITION_WEIGHT) + (c.Count * ISSUE_REPLY_WEIGHT)
                                                   }).OrderByDescending(b2 => b2.WeightedAverage).ToList<WeightModelReply>();

                return (from b in myWeightModel
                        select b.IssueReply).ToList<IssueReply>();

            }

            private class WeightModelIssue {
                public Issue Issue { get; set; }
                public double WeightedAverage { get; set; }
            }

            private class WeightModelReply {
                public IssueReply IssueReply { get; set; }
                public double WeightedAverage { get; set; }
            }
        }


        public IEnumerable<IssueReply> NewestIssueReplys() {
            return (from ir in theEntities.IssueReplys
                    where ir.Deleted == false
                    && ir.Issue.Deleted == false
                    select ir)
                    .OrderByDescending(i2 => i2.DateTimeStamp)
                    .ToList<IssueReply>();
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

        public IEnumerable<Issue> OfficialsIssueFeed(User aViewingUser, IEnumerable<string> aSelectedRoles) {
            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.UserId equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where u.Id != aViewingUser.Id
                     && i.Deleted == false
                    && aSelectedRoles.Contains(r.Name)
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> OfficialsIssueReplyFeed(User aViewingUser, IEnumerable<string> aSelectedRoles) {
            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where u.Id != aViewingUser.Id
                    && ir.Deleted == false
                    && aSelectedRoles.Contains(r.Name)
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<Issue> FilteredIssuesFeed(User aUser) {
            IEnumerable<FilteredCityState> myFilteredCityStates = FindFilteredCityStateForUser(aUser);
            IEnumerable<FilteredZipCode> myFilteredZipCodes = FindFilteredZipCodeForUser(aUser);

            IEnumerable<string> myCitys = (from f in myFilteredCityStates select f.City).ToList<string>();
            IEnumerable<string> myStates = (from f in myFilteredCityStates select f.State).ToList<string>();
            IEnumerable<int> myZipCodes = (from f in myFilteredZipCodes select f.ZipCode).ToList<int>();

            return (from i in theEntities.Issues
                    join u in theEntities.Users on i.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && i.Deleted == false
                    && (myCitys.Count<string>() > 1 ? myCitys.Contains(i.City) : true)
                    && (myStates.Count<string>() > 1 ? myStates.Contains(i.State) : true)
                    && (myZipCodes.Count<int>() > 1 ? myZipCodes.Contains(i.Zip.Value) : true)
                    select i).OrderByDescending(i => i.DateTimeStamp).ToList<Issue>();
        }

        public IEnumerable<IssueReply> FilteredIssueReplysFeed(User aUser) {
            IEnumerable<FilteredCityState> myFilteredCityStates = FindFilteredCityStateForUser(aUser);
            IEnumerable<FilteredZipCode> myFilteredZipCodes = FindFilteredZipCodeForUser(aUser);

            IEnumerable<string> myCitys = (from f in myFilteredCityStates select f.City).ToList<string>();
            IEnumerable<string> myStates = (from f in myFilteredCityStates select f.State).ToList<string>();
            IEnumerable<int> myZipCodes = (from f in myFilteredZipCodes select f.ZipCode).ToList<int>();

            return (from ir in theEntities.IssueReplys
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    where u.Id != aUser.Id
                    && ir.Deleted == false
                    && (myCitys.Count<string>() > 1 ? myCitys.Contains(ir.City) : true)
                    && (myStates.Count<string>() > 1 ? myStates.Contains(ir.State) : true)
                    && (myZipCodes.Count<int>() > 1 ? myZipCodes.Contains(ir.Zip.Value) : true)
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

        public void AddZipCodeFilter(User aUser, int aZipCode) {
            FilteredZipCode myFiltered = FilteredZipCode.CreateFilteredZipCode(0, aUser.Id, aZipCode);
            theEntities.AddToFilteredZipCodes(myFiltered);
        }

        public void AddCityStateFilter(User aUser, string aCity, string aState) {
            FilteredCityState myFiltered = FilteredCityState.CreateFilteredCityState(0, aUser.Id, aCity, aState);
            theEntities.AddToFilteredCityStates(myFiltered);
        }


        public bool ZipCodeFilterExists(User aUser, int aZipCode) {
            int myFilered = (from f in theEntities.FilteredZipCodes
                                       where f.User.Id == aUser.Id && f.ZipCode == aZipCode
                                       select f).Count<FilteredZipCode>();
            return myFilered > 0;
        }

        public bool CityStateFilterExists(User aUser, string aCity, string aState) {
            int myFilered = (from f in theEntities.FilteredCityStates
                             where f.User.Id == aUser.Id && f.City == aCity && f.State == aState
                             select f).Count<FilteredCityState>();
            return myFilered > 0;
        }
    }
}