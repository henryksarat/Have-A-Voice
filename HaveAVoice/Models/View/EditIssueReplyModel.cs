using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class EditIssueReplyModel {
        public int Id { get; set; }
        public string Reply { get; set; }
        public Disposition Disposition { get; set; }
    }
}