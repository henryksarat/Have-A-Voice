using System;
using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVMessageRepository {
        Message CreateMessage(int fromUserId, Message messageToCreate);
        IEnumerable<InboxMessage> GetMessagesForUser(User toUser);
        void DeleteMessages(List<Int32> messagesToDelete, User user);
        Message GetMessage(User aViewingUser, int messageId);

        Message CreateReply(int messageId, User user, string body);
        Message ChangeMessageViewedStateForMe(int messageId, User toUser, bool viewed);
        Message ChangeMessageViewedStateForThem(int messageId, User toUser, bool viewed);
    }
}
