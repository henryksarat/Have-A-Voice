using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.UserFeatures.Helpers {
    public class MessageHelper {
        public static List<InboxMessage> GenerateInbox(User aUser, IEnumerable<Message> aMessages, IEnumerable<Reply> aReplys) {
            //Get the last reply for a message for the user,
            //ordered by the reply DateTimeStamp or by the message DateTimeStamp
            return (from m in aMessages
                    let allReplys = aReplys.Where(r2 => m.Id == r2.MessageId).OrderByDescending(r3 => r3.DateTimeStamp).ToList<Reply>()
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
                        DateTimeStamp = (latestReply == null ? m.DateTimeStamp : latestReply.DateTimeStamp),
                        FromUserProfilePictureUrl = ProfilePictureHelper.ProfilePicture(m.FromUser)
                    }).ToList<InboxMessage>();
        }
    }
}