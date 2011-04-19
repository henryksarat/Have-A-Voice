using System;
using System.Collections.Generic;
using Social.Generic.Models;
using Social.Messaging.Helpers;
using Social.Messaging.Repositories;
using Social.Validation;

namespace Social.Messaging.Services {
    public class MessageService<T, U, V> : IMessageService<T, U, V> {
        private IValidationDictionary theValidationDictionary;
        private IMessageRepository<T, U, V> theRepository;

        public MessageService(IValidationDictionary aValidationDictionary, IMessageRepository<T, U, V> aRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        public bool CreateMessage(int fromUserId, AbstractMessageModel<U> messageToCreate) {
            if (!ValidateMessage(messageToCreate)) {
                return false;
            }
            theRepository.CreateMessage(fromUserId, messageToCreate.FromModel());
            return true;
        }

        public bool CreateMessage(int aFromUserId, int aToUserId, string aSubject, string aBody) {
            if (!ValidateMessage(aSubject, aBody)) {
                return false;
            }
            theRepository.CreateMessage(aFromUserId, aToUserId, aSubject, aBody);
            return true;
        }

        public IEnumerable<InboxMessage> GetMessagesForUser(AbstractUserModel<T> toUser) {
            IEnumerable<AbstractMessageModel<U>> myMessages = theRepository.GetAllMessagesAsAbstract();
            IEnumerable<AbstractReplyModel<V>> myReplys = theRepository.GetAllReplysAsAbstract();
            
            return MessageHelper<T, U, V>.GenerateInbox(toUser, myMessages, myReplys);
        }

        public void DeleteMessages(List<Int32> messagesToDelete, T user) {
            theRepository.DeleteMessages(messagesToDelete, user);
        }

        public U GetMessage(int messageId, T user) {
            U message = theRepository.GetMessage(user, messageId);
            theRepository.ChangeMessageViewedStateForMe(messageId, user, true);
            return message;
        }

        public bool CreateReply(int messageId, T user, string body) {
            if (!ValidateReply(body)) {
                return false;
            }

            AbstractMessageModel<U> message = theRepository.CreateReply(messageId, user, body);
            theRepository.ChangeMessageViewedStateForThem(message.Id, user, false);

            return true;
        }

        public bool AllowedToViewMessageThread(T aUser, int aMessageId) {
            return theRepository.UserInvolvedInMessage(aUser, aMessageId);
        }

        public bool ValidateMessage(AbstractMessageModel<U> message) {

            if (message.Subject.Trim().Length == 0) {
                theValidationDictionary.AddError("Subject", message.Subject.Trim(), "Subject is required.");
            }
            if (message.Body.Trim().Length == 0) {
                theValidationDictionary.AddError("Body", message.Body.Trim(), "Body is required.");
            }

            return theValidationDictionary.isValid;
        }

        public bool ValidateMessage(string aSubject, string aBody) {

            if (aSubject.Trim().Length == 0) {
                theValidationDictionary.AddError("Subject", aSubject.Trim(), "Subject is required.");
            }
            if (aBody.Trim().Length == 0) {
                theValidationDictionary.AddError("Body", aBody.Trim(), "Body is required.");
            }

            return theValidationDictionary.isValid;
        }

        public bool ValidateReply(string reply) {
            if (reply.Trim().Length == 0) {
                theValidationDictionary.AddError("Reply", reply.Trim(), "Reply is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
