using System;
using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Messaging.Repositories {
    public interface IMessageRepository<T, U, V> {
        IEnumerable<U> GetAllMessages();
        IEnumerable<AbstractMessageModel<U>> GetAllMessagesAsAbstract();
        U CreateMessage(int fromUserId, U messageToCreate);
        void DeleteMessages(List<Int32> messagesToDelete, T user);
        U GetMessage(T aViewingUser, int messageId);
        bool UserInvolvedInMessage(T aUser, int aMessageId);

        IEnumerable<V> GetAllReplys();
        IEnumerable<AbstractReplyModel<V>> GetAllReplysAsAbstract();
        AbstractMessageModel<U> CreateReply(int messageId, T user, string body);
        U ChangeMessageViewedStateForMe(int messageId, T toUser, bool viewed);
        U ChangeMessageViewedStateForThem(int messageId, T toUser, bool viewed);
    }
}
