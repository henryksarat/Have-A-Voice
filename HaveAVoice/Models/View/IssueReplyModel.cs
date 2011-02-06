using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueReplyModel {
        public Issue Issue { get; set; }
        public int Id { get; set; }
        public User User { get; set; }
        public string Reply { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int CommentCount { get; set; }
        public bool Anonymous { get; set; }
        public bool HasDisposition { get; set; }
        public int IssueStance { get; set; }
        public Disposition Disposition { get; private set; }
        public IssueStanceFilter IssueStanceFilter { get; private set; }
        public PersonFilter PersonFilter { get; private set; }

        public int TempDispositionHolder {
            get {
                return (int)Disposition;
            }
            set {
                if (value == (int)Disposition.Like) {
                    Disposition = Disposition.Like;
                    IssueStanceFilter = IssueStanceFilter.Agree;
                } else if (value == (int)Disposition.Dislike) {
                    Disposition = Disposition.Dislike;
                    IssueStanceFilter = IssueStanceFilter.Disagree;
                } else {
                    Disposition = Disposition.None;
                    IssueStanceFilter = IssueStanceFilter.All;
                }
            }
        }

        public int TempPersonFilterHolder {
            get {
                return (int)PersonFilter;
            }
            set {
                if (value == (int)PersonFilter.People) {
                    PersonFilter = PersonFilter.People;
                } else if (value == (int)PersonFilter.Politicians) {
                    PersonFilter = PersonFilter.Politicians;
                } else if (value == (int)PersonFilter.PoliticalCandidates) {
                    PersonFilter = PersonFilter.PoliticalCandidates;
                } else {
                    PersonFilter = PersonFilter.All;
                }
            }
        }
    }
}
