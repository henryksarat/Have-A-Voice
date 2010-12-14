using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueModel {
        public Issue Issue { get; set; }
        public IEnumerable<IssueReplyModel> UserReplys { get; set; }
        public IEnumerable<IssueReplyModel> OfficialReplys { get; set; }
        public string Comment { get; set; }
        public bool Anonymous { get; set; }
        public Disposition Disposition { get; set; }

        public IssueModel(Issue aIssue, IEnumerable<IssueReplyModel> aUserReplys,  
            IEnumerable<IssueReplyModel> aOfficialReplys) {
            this.Issue = aIssue;
            this.UserReplys = aUserReplys;
            this.OfficialReplys = aOfficialReplys;
            this.Comment = string.Empty;
            this.Anonymous = false;
            this.Disposition = Disposition.NONE;
        }
    }
}