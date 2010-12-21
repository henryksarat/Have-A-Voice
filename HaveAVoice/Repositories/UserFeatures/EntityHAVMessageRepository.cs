﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVMessageRepository : HAVBaseRepository, IHAVMessageRepository {

        public Message CreateMessage(int fromUserId, Message messageToCreate) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            messageToCreate.FromUserId = fromUserId;
            messageToCreate.DateTimeStamp = DateTime.UtcNow;

            GetEntities().AddToMessages(messageToCreate);
            GetEntities().SaveChanges();

            return messageToCreate;
        }

        public IEnumerable<InboxMessage> GetMessagesForUser(User toUser) {
            IEnumerable<Reply> replys = GetEntities().Replys;
            IEnumerable<Message> messages = GetEntities().Messages;
            return Helpers.GenerateInbox(toUser, messages.ToList(), replys.ToList());
        }

        public void DeleteMessages(List<Int32> messagesToDelete, User user) {
            foreach (Int32 messageId in messagesToDelete) {
                var message = GetMessage(messageId);

                if (message.ToUserId == user.Id) {
                    message.ToDeleted = true;
                } else if (message.FromUserId == user.Id) {
                    message.FromDeleted = true;
                }

                GetEntities().ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            }

            GetEntities().SaveChanges();
        }

        public Message GetMessage(User aViewingUser, int aMessageId) {
            return (from m in GetEntities().Messages.Include("FromUser").Include("Replys.User")
                    where m.Id == aMessageId
                    && ((aViewingUser.Id == m.ToUserId && !m.ToDeleted)  || (aViewingUser.Id == m.FromUserId && !m.FromDeleted))
                    select m).FirstOrDefault<Message>();
        }

        public Message CreateReply(int messageId, User user, string body) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            User replyUser = userRepository.GetUser(user.Id);
            Message message = GetMessage(messageId);

            Reply reply = new Reply();
            reply.Message = message;
            reply.User = replyUser;
            reply.Body = body;
            reply.DateTimeStamp = DateTime.UtcNow;

            bool updatedRepliedTo = false;

            if (message.ToUserId == replyUser.Id && message.RepliedTo == false) {
                message.RepliedTo = true;
                updatedRepliedTo = true;
            }

            if (updatedRepliedTo == true) {
                GetEntities().ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            }

            GetEntities().AddToReplys(reply);
            GetEntities().SaveChanges();

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

            GetEntities().ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            GetEntities().SaveChanges();

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

            GetEntities().ApplyCurrentValues(message.EntityKey.EntitySetName, message);
            GetEntities().SaveChanges();

            return message;
        }

        public static class Helpers {
            public static List<InboxMessage> GenerateInbox(User aUser, List<Message> aMessages, List<Reply> aReplys) {
                //Get the last reply for a message for the user,
                //ordered by the reply DateTimeStamp or by the message DateTimeStamp
                return (from m in aMessages
                        let allReplys = aReplys.Where(r2 => m.Id == r2.Message.Id).OrderByDescending(r3 => r3.DateTimeStamp).ToList<Reply>()
                        let latestReply = allReplys.FirstOrDefault<Reply>()
                        where (m.ToDeleted == false
                        && m.ToUserId == aUser.Id)
                        || (m.FromDeleted == false
                        && m.FromUserId == aUser.Id
                        && allReplys.Count() > 0)
                        orderby (latestReply != null ? latestReply.DateTimeStamp : m.DateTimeStamp) descending
                        select new InboxMessage {
                            MessageId = m.Id,
                            Subject = m.Subject,
                            FromUsername = m.FromUser.Username,
                            FromUserId = m.FromUser.Id,
                            LastReply = (latestReply == null ? m.Body : latestReply.Body),
                            Viewed = (m.ToUserId == aUser.Id ? m.ToViewed : m.FromViewed),
                            DateTimeStamp = (latestReply == null ? m.DateTimeStamp : latestReply.DateTimeStamp)
                        }).ToList<InboxMessage>();    
            }
        }

        private Message GetMessage(int aMessageId) {
            return (from m in GetEntities().Messages.Include("FromUser").Include("Replys.User")
                    where m.Id == aMessageId
                    select m).FirstOrDefault<Message>();
        }
    }
}
