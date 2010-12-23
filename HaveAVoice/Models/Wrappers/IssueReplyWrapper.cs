using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models {
    public class IssueReplyWrapper : BasicTextModelWrapper{
        public int Disposition {get; set;}
        
        public IssueReply ToModel() {
            return IssueReply.CreateIssueReply(Id, 0, 0, Body, DateTime.UtcNow, false, Disposition, false);
        }

        public static IssueReplyWrapper Build(IssueReply aReply) {
            return new IssueReplyWrapper() {
                Id = aReply.Id,
                Body = aReply.Reply,
                Disposition = aReply.Disposition
            };
        }
    }
}