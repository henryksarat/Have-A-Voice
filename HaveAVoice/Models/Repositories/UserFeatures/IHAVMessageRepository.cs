using System;
using System.Collections.Generic;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public interface IHAVMessageRepository {
        Message CreateMessage(int toUserId, int fromUserId, Message messageToCreate);
        IEnumerable<InboxMessage> GetMessagesForUser(User toUser);
        void DeleteMessages(List<Int32> messagesToDelete, User user);
        Message GetMessage(int messageId);

        Message CreateReply(int messageId, User user, string body);
        Message ChangeMessageViewedStateForMe(int messageId, User toUser, bool viewed);
        Message ChangeMessageViewedStateForThem(int messageId, User toUser, bool viewed);
    }
}
