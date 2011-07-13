using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialWrappers {
    public class SocialMessageReplyModel : AbstractReplyModel<MessageReply> {
        public static SocialMessageReplyModel Create(MessageReply anExternal) {
            return new SocialMessageReplyModel(anExternal);
        }

        public override MessageReply FromModel() {
            return MessageReply.CreateMessageReply(Id, ReplyUserId, MessageId, Body, DateTimeStamp);
        }

        private SocialMessageReplyModel(MessageReply anExternal) {
            Id = anExternal.Id;
            ReplyUserId = anExternal.ReplyUserId;
            MessageId = anExternal.MessageId;
            Body = anExternal.Body;
            DateTimeStamp = anExternal.DateTimeStamp;

            Model = anExternal;
        }
    }
}