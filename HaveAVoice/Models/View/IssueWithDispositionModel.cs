using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueWithDispositionModel {
        public Issue Issue { get; set; }
        public bool HasDisposition { get; set; }
        public int TotalAgrees { get; set; }
        public int TotalDisagrees { get; set; }

        public IssueWithDispositionModel() { }

        public IssueWithDispositionModel(Issue anIssue) {
            Issue = anIssue;
            TotalAgrees = (from d in anIssue.IssueDispositions
                           where d.Disposition == (int)Disposition.Like
                           select d).Count<IssueDisposition>();
            TotalDisagrees = (from d in anIssue.IssueDispositions
                              where d.Disposition == (int)Disposition.Dislike
                              select d).Count<IssueDisposition>();

        }
    }
}