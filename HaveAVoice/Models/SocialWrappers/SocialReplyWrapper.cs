using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialReplyWrapper : AbstractReplyModel<Reply> {
        public static SocialReplyWrapper Create(Reply anExternal) {
            return new SocialReplyWrapper(anExternal);
        }
        
        public override Reply FromModel() {
            return Reply.CreateReply(Id, ReplyUserId, MessageId, Body, DateTimeStamp);
        }

        private SocialReplyWrapper(Reply anExternal) {
            Id = anExternal.Id;
            ReplyUserId = anExternal.ReplyUserId;
            MessageId = anExternal.MessageId;
            Body = anExternal.Body;
            DateTimeStamp = anExternal.DateTimeStamp;
        }
    }
}