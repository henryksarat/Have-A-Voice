using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueReplyFeedModel : FeedModel {
        public IEnumerable<IssueReplyComment> IssueReplyComments { get; set; }
        public PersonFilter PersonFilter { get; set; }
        public string State;
        public string City;
        public Issue Issue { get; set; }
        public string Reply { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool HasDisposition { get; set; }
        public int TotalComments { get; set; }
        public string IconType {
            get {
                string myCssIconClass = "resident";

                if (PersonFilter == PersonFilter.Politicians) {
                    myCssIconClass = "politician";
                }
                if (PersonFilter == PersonFilter.PoliticalCandidates) {
                    myCssIconClass = "candidate";
                }

                return myCssIconClass;
            }
        }

        public IssueReplyFeedModel(User aUser) : base(aUser) {
            IssueReplyComments = new List<IssueReplyComment>();
        }
    }
}