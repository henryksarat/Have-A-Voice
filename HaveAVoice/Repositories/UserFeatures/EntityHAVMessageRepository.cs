using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVMessageRepository : IHAVMessageRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public Message CreateMessage(int fromUserId, Message messageToCreate) {
            messageToCreate.FromUserId = fromUserId;
            messageToCreate.DateTimeStamp = DateTime.UtcNow;

            theEntities.AddToMessages(messageToCreate);
            theEntities.SaveChanges();

            return messageToCreate;
        }

        public IEnumerable<InboxMessage> GetMessagesForUser(User toUser) {
            IEnumerable<Reply> replys = theEntities.Replys;
            IEnumerable<Message> messages = theEntities.Messages;
            return MessageHelper.GenerateInbox(toUser, messages.ToList(), replys.ToList());
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
            return (from m in theEntities.Messages.Include("FromUser").Include("Replys.User")
                    where m.Id == aMessageId
                    && ((aViewingUser.Id == m.ToUserId && !m.ToDeleted)  || (aViewingUser.Id == m.FromUserId && !m.FromDeleted))
                    select m).FirstOrDefault<Message>();
        }

        public Message CreateReply(int messageId, User user, string body) {
            Message message = GetMessage(messageId);

            Reply myReply = Reply.CreateReply(0, user.Id, messageId, body, DateTime.UtcNow);

            bool updatedRepliedTo = false;

            if (message.ToUserId == user.Id && message.RepliedTo == false) {
                message.RepliedTo = true;
                updatedRepliedTo = true;
            }

            if (updatedRepliedTo == true) {
                theEntities.ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            }

            theEntities.AddToReplys(myReply);
            theEntities.SaveChanges();

            return message;
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
            return (from m in theEntities.Messages.Include("FromUser").Include("Replys.User")
                    where m.Id == aMessageId
                    select m).FirstOrDefault<Message>();
        }
    }
}
