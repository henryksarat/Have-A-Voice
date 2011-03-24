using System;

namespace Social.Generic.Models {
    public abstract class AbstractAuditedModel<T> : AbstractSocialModel<T> {
        public int? UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTimeStamp { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedByUserId { get; set; }

        public abstract T FromModel();
    }
}
