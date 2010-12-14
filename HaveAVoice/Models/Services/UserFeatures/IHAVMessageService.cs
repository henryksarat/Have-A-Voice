using System;
using System.Collections.Generic;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVMessageService {
        bool CreateMessage(int toUserId, int fromUserId, Message messageToCreate);
        IEnumerable<InboxMessage> GetMessagesForUser(User toUser);
        void DeleteMessages(List<Int32> messagesToDelete, User user);
        Message GetMessage(int messageId, User user);
        bool CreateReply(int messageId, User user, string body);
        bool AllowedToViewMessageThread(User user, int messageId);
    }
}
