using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.User.Models;

namespace Social.Messaging.Helpers {
    public static class MessageHelper<T, U, V> {
        public static List<InboxMessage<T>> GenerateInbox(AbstractUserModel<T> aUser, 
                                                       IEnumerable<AbstractMessageModel<U, T>> aMessages, 
                                                       IEnumerable<AbstractReplyModel<V>> aReplys) {
            //Get the last reply for a message for the user,
            //ordered by the reply DateTimeStamp or by the message DateTimeStamp
            return (from m in aMessages
                    let allReplys = aReplys.Where(r2 => m.Id == r2.MessageId).OrderByDescending(r3 => r3.DateTimeStamp).ToList()
                    let latestReply = allReplys.FirstOrDefault()
                    where (m.ToDeleted == false
                    && m.ToUserId == aUser.Id)
                    || (m.FromDeleted == false
                    && m.FromUserId == aUser.Id
                    && allReplys.Count() > 0)
                    orderby (latestReply != null ? latestReply.DateTimeStamp : m.DateTimeStamp) descending
                    select new InboxMessage<T> {
                        MessageId = m.Id,
                        Subject = m.Subject,
                        FromUser = aUser.Id == m.FromUserId ? m.ToUser : m.FromUser,
                        LastReply = (latestReply == null ? m.Body : latestReply.Body),
                        Viewed = (m.ToUserId == aUser.Id ? m.ToViewed : m.FromViewed),
                        DateTimeStamp = (latestReply == null ? m.DateTimeStamp : latestReply.DateTimeStamp),
                    }).ToList<InboxMessage<T>>();
        }
    }
}
