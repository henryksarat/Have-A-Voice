using System;
using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVMessageService : HAVBaseService, IHAVMessageService {
        private IValidationDictionary theValidationDictionary;
        private IHAVMessageRepository theRepository;

        public HAVMessageService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVMessageRepository(), new HAVBaseRepository()) { }

        public HAVMessageService(IValidationDictionary aValidationDictionary, IHAVMessageRepository aRepository, 
                                                IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        public bool CreateMessage(int fromUserId, Message messageToCreate) {
            if (!ValidateMessage(messageToCreate)) {
                return false;
            }
            theRepository.CreateMessage(fromUserId, messageToCreate);
            return true;
        }

        public IEnumerable<InboxMessage> GetMessagesForUser(User toUser) {
            IEnumerable<Message> myMessages = theRepository.GetAllMessages();
            IEnumerable<Reply> myReplys = theRepository.GetAllReplys();

            return MessageHelper.GenerateInbox(toUser, myMessages, myReplys);
        }

        public void DeleteMessages(List<Int32> messagesToDelete, User user) {
            theRepository.DeleteMessages(messagesToDelete, user);
        }

        public Message GetMessage(int messageId, User user) {
            Message message = theRepository.GetMessage(user, messageId);
            theRepository.ChangeMessageViewedStateForMe(messageId, user, true);
            return message;
        }

        public bool CreateReply(int messageId, User user, string body) {
            if (!ValidateReply(body)) {
                return false;
            }

            Message message = theRepository.CreateReply(messageId, user, body);
            theRepository.ChangeMessageViewedStateForThem(message.Id, user, false);

            return true;
        }

        public bool AllowedToViewMessageThread(User user, int messageId) {
            Message message = theRepository.GetMessage(user, messageId);
            return (message.ToUserId == user.Id || message.FromUserId == user.Id);
        }

        public bool ValidateMessage(Message message) {

            if (message.Subject.Trim().Length == 0) {
                theValidationDictionary.AddError("Subject", message.Subject.Trim(), "Subject is required.");
            }
            if (message.Body.Trim().Length == 0) {
                theValidationDictionary.AddError("Body", message.Body.Trim(), "Body is required.");
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
