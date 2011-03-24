using System;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    public abstract class AbstractBoardReplyModel<T> : AbstractAuditedModel<T> {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BoardId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Message { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public override abstract T FromModel();
    }
}
