using System;
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

        public Message GetMessage(int messageId) {
            return (from c in GetEntities().Messages.Include("FromUser").Include("Replys.User")
                    where c.Id == messageId
                    select c).FirstOrDefault<Message>();
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
                //Get the last comment for a message for the user,
                //ordered by the comment DateTimeStamp, then by the message DateTimeStamp
                return (from m in aMessages
                        let r = aReplys.Where(r2 => m.Id == r2.Message.Id).OrderByDescending(r3 => r3.DateTimeStamp).ToList<Reply>()
                        let r2 = r.FirstOrDefault<Reply>()
                        where (m.ToDeleted == false
                        && m.ToUserId == aUser.Id)
                        || (m.FromDeleted == false
                        && m.FromUserId == aUser.Id
                        && r.Count() > 0)
                        orderby m.DateTimeStamp descending
                        select new InboxMessage {
                            MessageId = m.Id,
                            Subject = m.Subject,
                            FromUsername = m.FromUser.Username,
                            FromUserId = m.FromUser.Id,
                            LastReply = (r2 == null ? m.Body : r2.Body),
                            Viewed = (m.ToUserId == aUser.Id ? m.ToViewed : m.FromViewed),
                            DateTimeStamp = (r2 == null ? m.DateTimeStamp : r2.DateTimeStamp)
                        }).ToList<InboxMessage>();    
            }
        }
    }
}
