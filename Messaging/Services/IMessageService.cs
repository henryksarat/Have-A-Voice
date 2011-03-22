using System;
using System.Collections.Generic;
using Social.Generic.Models;
using Social.User.Models;

namespace Social.Messaging.Services {
    public interface IMessageService<T, U, V> {
        bool CreateMessage(int fromUserId, AbstractMessageModel<U> messageToCreate);
        IEnumerable<InboxMessage> GetMessagesForUser(AbstractUserModel<T> toUser);
        void DeleteMessages(List<Int32> messagesToDelete, T user);
        U GetMessage(int messageId, T user);
        bool CreateReply(int messageId, T user, string body);
        bool AllowedToViewMessageThread(T user, int messageId);
    }
}
