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
            IEnumerable<Issue> myIssues = GetOrderedIssues();
            IEnumerable<int> myIssueIds = (from i in myIssues
                                           select i.Id).ToList<int>();
            IEnumerable<IssueDisposition> myIssueDispositions = (from d in theEntities.IssueDispositions
                                                                 where myIssueIds.Contains(d.IssueId)
                                                                 select d).ToList<IssueDisposition>();
            IEnumerable<IssueReply> myIssueReplys = GetOrderedIssueReplys();

            return Helpers.CalculatedWithWeights(myIssues.ToList(), myIssueReplys.ToList(), myIssueDispositions.ToList(), aDisposition);
        }

        public IEnumerable<IssueReply> GetMostPopularIssueReplys() {
            IEnumerable<IssueReply> myReplys = GetOrderedIssueReplys();
            IEnumerable<int> myReplyIds = (from ir in myReplys
                                           select ir.Id).ToList<int>();
            IEnumerable<IssueReplyComment> myComments = (from c in theEntities.IssueReplyComments
                                                         where myReplyIds.Contains(c.IssueReplyId)
                                                         select c).ToList<IssueReplyComment>();
            IEnumerable<IssueReplyDisposition> myDispositions = (from d in theEntities.IssueReplyDispositions
                                                                 where myReplyIds.Contains(d.IssueReplyId)
                                                                 select d).ToList<IssueReplyDisposition>();

            return Helpers.CalculatedWithWeights(myReplys.ToList(), myComments.ToList(), myDispositions.ToList());
        }

        private IEnumerable<Issue> GetOrderedIssues() {
            return (from i in theEntities.Issues
                    where i.Deleted == false
                    select i).OrderByDescending(i2 => i2.DateTimeStamp).ToList<Issue>();
        }

        private IEnumerable<IssueReply> GetOrderedIssueReplys() {
            return (from ir in theEntities.IssueReplys
                    where ir.Deleted == false
                    select ir).ToList<IssueReply>();
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
                        select new IssueWithDispositionModel(b.Issue) {
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
    }
}