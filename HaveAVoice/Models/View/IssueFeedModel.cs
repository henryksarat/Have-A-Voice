using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueFeedModel : FeedModel {
        public IEnumerable<IssueReply> IssueReplys { get; set; }
        public PersonFilter PersonFilter { get; set; }
        public string State;
        public string City;
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool HasDisposition { get; set; }
        public int TotalReplys { get; set; }
        public string IconType {
            get {
                string myCssIconClass = "resident";

                if (PersonFilter == PersonFilter.Politicians) {
                    myCssIconClass = "politician";
                }
                if (PersonFilter == PersonFilter.Politicians) {
                    myCssIconClass = "candidate";
                }

                return myCssIconClass;
            }
        }

        public IssueFeedModel(User aUser) : base(aUser) {
            IssueReplys = new List<IssueReply>();
        }
    }
}