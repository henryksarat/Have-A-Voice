using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVHomeRepository : HAVBaseRepository, IHAVHomeRepository {
        private static double ISSUE_DISPOSITION_WEIGHT = 0.2;
        private static double ISSUE_REPLY_WEIGHT = 0.8;

        public IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition) {
            IEnumerable<Issue> myIssues = GetEntities().Issues;
            IEnumerable<IssueDisposition> myIssueDispositions = GetEntities().IssueDispositions;
            IEnumerable<IssueReply> myIssueReplys = GetEntities().IssueReplys;

            return Helpers.CalculatedWithWeights(myIssues.ToList(), myIssueReplys.ToList(), myIssueDispositions.ToList(), aDisposition);
        }

        public IEnumerable<IssueReply> GetMostPopularIssueReplys() {
            IEnumerable<IssueReply> myReplys = GetEntities().IssueReplys;
            IEnumerable<IssueReplyComment> myComments = GetEntities().IssueReplyComments;
            IEnumerable<IssueReplyDisposition> myDispositions = GetEntities().IssueReplyDispositions;

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
            return (from ir in GetEntities().IssueReplys
                    where ir.Deleted == false
                    && ir.Issue.Deleted == false
                    select ir)
                    .OrderByDescending(i2 => i2.DateTimeStamp)
                    .ToList<IssueReply>();
        }

        public IEnumerable<IssueReply> FanFeed(User aUser) {
            return (from ir in GetEntities().IssueReplys
                    join u in GetEntities().Users on ir.User.Id equals u.Id
                    join f in GetEntities().Fans on u.Id equals f.SourceUser.Id
                    where f.FanUserId == aUser.Id
                    && ir.Deleted == false
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }


        public IEnumerable<IssueReply> OfficialsFeed(IEnumerable<string> aSelectedRoles) {
            return (from ir in GetEntities().IssueReplys
                    join u in GetEntities().Users on ir.User.Id equals u.Id
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    where ir.Deleted == false
                    where aSelectedRoles.Contains(r.Name)
                    select ir).OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReply>();
        }

        public IEnumerable<IssueReply> FilteredFeed(User aUser) {
            throw new NotImplementedException();
        }

        public void AddZipCodeFilter(User aUser, int aZipCode) {
            FilteredZipCode myFiltered = FilteredZipCode.CreateFilteredZipCode(0, aUser.Id, aZipCode);
            GetEntities().AddToFilteredZipCodes(myFiltered);
        }

        public void AddCityStateFilter(User aUser, string aCity, string aState) {
            FilteredCityState myFiltered = FilteredCityState.CreateFilteredCityState(0, aUser.Id, aCity, aState);
            GetEntities().AddToFilteredCityStates(myFiltered);
        }


        public bool ZipCodeFilterExists(User aUser, int aZipCode) {
            int myFilered = (from f in GetEntities().FilteredZipCodes
                                       where f.User.Id == aUser.Id && f.ZipCode == aZipCode
                                       select f).Count<FilteredZipCode>();
            return myFilered > 0;
        }

        public bool CityStateFilterExists(User aUser, string aCity, string aState) {
            int myFilered = (from f in GetEntities().FilteredCityStates
                             where f.User.Id == aUser.Id && f.City == aCity && f.State == aState
                             select f).Count<FilteredCityState>();
            return myFilered > 0;
        }
    }
}