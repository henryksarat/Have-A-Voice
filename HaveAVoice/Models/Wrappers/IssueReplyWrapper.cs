using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models {
    public class IssueReplyWrapper : BasicTextModelWrapper{
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZipCode { get; set; }

        public int Disposition {get; set;}
        
        public IssueReply ToModel() {
            return IssueReply.CreateIssueReply(Id, 0, 0, Body, City, State, int.Parse(ZipCode), Disposition, false, DateTime.UtcNow, false);
        }

        public static IssueReplyWrapper Build(IssueReply aReply) {
            return new IssueReplyWrapper() {
                Id = aReply.Id,
                Body = aReply.Reply,
                Disposition = aReply.Disposition,
                City = aReply.City,
                State = aReply.State
            };
        }
    }
}