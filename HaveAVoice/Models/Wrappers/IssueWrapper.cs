using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models.Wrappers {
    public class IssueWrapper {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZipCode { get; set; }

        public Issue ToModel() {
            return Issue.CreateIssue(Id, Title, Description, City, State, int.Parse(ZipCode), DateTime.UtcNow, 0, false);
        }
    }
}