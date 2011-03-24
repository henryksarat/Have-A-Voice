using System;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    public abstract class AbstractBoardModel<T> : AbstractAuditedModel<T> {
        public int Id { get; set; }
        public int OwnerUserId { get; set; }
        public int PostedUserId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Message { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public override abstract T FromModel();
    }
}
