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

        public Issue ToModel() {
            return Issue.CreateIssue(Id, Title, Description, DateTime.UtcNow, 0, false);
        }

        public static IssueWrapper Build(Issue aBoard) {
            return new IssueWrapper() {
                Id = aBoard.Id,
                Title = aBoard.Title,
                Description = aBoard.Description
            };
        }
    }
}