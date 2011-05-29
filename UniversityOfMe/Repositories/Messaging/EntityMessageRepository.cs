using System;
using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.Messaging.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.SocialWrappers;

namespace UniversityOfMe.Repositories.Messaging {
    public class EntityMessageRepository : IMessageRepository<User, Message, MessageReply> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Message> GetAllMessages() {
            return theEntities.Messages.ToList<Message>();
        }

        public IEnumerable<AbstractMessageModel<Message, User>> GetAllMessagesAsAbstract() {
            IEnumerable<Message> myMessages =  (from m in theEntities.Messages
                                                select m).ToList();

            IEnumerable<AbstractMessageModel<Message, User>> myAbstract = (from m in myMessages
                                                                     select SocialMessageModel.Create(m))
                                                                     .ToList<AbstractMessageModel<Message, User>>();
            return myAbstract;

        }

        public Message CreateMessage(int fromUserId, Message messageToCreate) {
            messageToCreate.FromUserId = fromUserId;
            messageToCreate.DateTimeStamp = DateTime.UtcNow;

            theEntities.AddToMessages(messageToCreate);
            theEntities.SaveChanges();

            return messageToCreate;
        }

        public Message CreateMessage(int aFromUserId, int aToUserId, string aSubject, string aBody) {
            Message myMessage = Message.CreateMessage(0, aToUserId, aFromUserId, aSubject, aBody, DateTime.UtcNow, false, false, false, false, false);
            theEntities.AddToMessages(myMessage);
            theEntities.SaveChanges();
            return myMessage;
        }

        public void DeleteMessages(List<Int32> messagesToDelete, User user) {
            foreach (Int32 messageId in messagesToDelete) {
                var message = GetMessage(messageId);

                if (message.ToUserId == user.Id) {
                    message.ToDeleted = true;
                } else if (message.FromUserId == user.Id) {
                    message.FromDeleted = true;
                }

                theEntities.ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            }

            theEntities.SaveChanges();
        }

        public Message GetMessage(User aViewingUser, int aMessageId) {
            return (from m in theEntities.Messages.Include("FromUser").Include("MessageReplies.User")
                    where m.Id == aMessageId
                    && ((aViewingUser.Id == m.ToUserId && !m.ToDeleted) || (aViewingUser.Id == m.FromUserId && !m.FromDeleted))
                    select m).FirstOrDefault<Message>();
        }

        public bool UserInvolvedInMessage(User aUser, int aMessageId) {
            Message myMessage = GetMessage(aMessageId);
            return myMessage.FromUserId == aUser.Id || myMessage.ToUserId == aUser.Id;
        }

        public IEnumerable<MessageReply> GetAllReplys() {
            return theEntities.MessageReplies.ToList<MessageReply>();
        }

        public IEnumerable<AbstractReplyModel<MessageReply>> GetAllReplysAsAbstract() {
            IEnumerable<MessageReply> myReplies = (from r in theEntities.MessageReplies
                                                   select r).ToList<MessageReply>();
            IEnumerable<AbstractReplyModel<MessageReply>> myAbstracts = (from r in myReplies
                                                                         select SocialMessageReplyModel.Create(r))
                                                                         .ToList<AbstractReplyModel<MessageReply>>();
            return myAbstracts;
        }

        public AbstractMessageModel<Message, User> CreateReply(int messageId, User user, string body) {
            Message message = GetMessage(messageId);

            MessageReply myReply = MessageReply.CreateMessageReply(0, user.Id, messageId, body, DateTime.UtcNow);

            bool updatedRepliedTo = false;

            if (message.ToUserId == user.Id && message.RepliedTo == false) {
                message.RepliedTo = true;
                updatedRepliedTo = true;
            }

            if (updatedRepliedTo == true) {
                theEntities.ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            }

            theEntities.AddToMessageReplies(myReply);
            theEntities.SaveChanges();

            return SocialMessageModel.Create(message);
        }

        public Message ChangeMessageViewedStateForMe(int messageId, User toUser, bool viewed) {
            Message message = GetMessage(messageId);
            if (message.FromUserId == toUser.Id) {
                message.FromViewed = viewed;
                message.FromDeleted = false;
            } else if (message.ToUserId == toUser.Id) {
                message.ToViewed = viewed;
                message.FromDeleted = false;
            }

            theEntities.ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            theEntities.SaveChanges();

            return message;
        }

        public Message ChangeMessageViewedStateForThem(int messageId, User toUser, bool viewed) {
            Message message = GetMessage(messageId);
            if (message.FromUserId == toUser.Id) {
                message.ToViewed = viewed;
                message.ToDeleted = false;
            } else if (message.ToUserId == toUser.Id) {
                message.FromViewed = viewed;
                message.FromDeleted = false;
            }

            theEntities.ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            theEntities.SaveChanges();

            return message;
        }

        private Message GetMessage(int aMessageId) {
            return (from m in theEntities.Messages.Include("FromUser").Include("MessageReplies.User")
                    where m.Id == aMessageId
                    select m).FirstOrDefault<Message>();
        }
    }
}
