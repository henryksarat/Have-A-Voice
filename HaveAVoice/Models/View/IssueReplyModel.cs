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
        public PersonFilter PersonFilter { get; private set; }

        public int TempDispositionHolder {
            set {
                if (value == (int)Helpers.Enums.Disposition.Like) {
                    Disposition = Helpers.Enums.Disposition.Like;
                } else if (value == (int)Helpers.Enums.Disposition.Dislike) {
                    Disposition = Helpers.Enums.Disposition.Dislike;
                } else {
                    Disposition = Helpers.Enums.Disposition.None;
                }
            }
        }

        public int TempPersonFilterHolder {
            set {
                if (value == (int)Helpers.Enums.PersonFilter.People) {
                    PersonFilter = Helpers.Enums.PersonFilter.People;
                } else if (value == (int)Helpers.Enums.PersonFilter.Politicians) {
                    PersonFilter = Helpers.Enums.PersonFilter.Politicians;
                } else {
                    PersonFilter = Helpers.Enums.PersonFilter.All;
                }
            }
        }
    }
}
