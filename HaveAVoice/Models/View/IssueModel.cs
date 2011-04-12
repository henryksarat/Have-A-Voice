using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models.View {
    public class IssueModel {
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
        public IEnumerable<IssueReplyModel> Replys { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Reply { get; set; }
        public bool Anonymous { get; set; }
        public Disposition Disposition { get; set; }
        public int TotalAgrees { get; set; }
        public int TotalDisagrees { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public int UserId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int Zip { get; set; }

        public IssueModel() { }

        public IssueModel(Issue aIssue, IEnumerable<IssueReplyModel> aReplys) {
            this.Issue = aIssue;
            this.Replys = aReplys;
            this.Reply = string.Empty;
            this.Anonymous = false;
            this.Disposition = Disposition.None;
            TotalAgrees = (from d in Issue.IssueDispositions
                            where d.Disposition == (int)Disposition.Like
                            select d).Count<IssueDisposition>();


            TotalDisagrees = (from d in Issue.IssueDispositions
                                where d.Disposition == (int)Disposition.Dislike
                                select d).Count<IssueDisposition>();
        }
    }
}