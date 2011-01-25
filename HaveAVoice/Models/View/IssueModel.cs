﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueModel {
        public Issue Issue { get; set; }
        public IEnumerable<IssueReplyModel> Replys { get; set; }
        public string Comment { get; set; }
        public bool Anonymous { get; set; }
        public Disposition Disposition { get; set; }

        public IssueModel(Issue aIssue, IEnumerable<IssueReplyModel> aReplys) {
            this.Issue = aIssue;
            this.Replys = aReplys;
            this.Comment = string.Empty;
            this.Anonymous = false;
            this.Disposition = Disposition.None;
        }
    }
}