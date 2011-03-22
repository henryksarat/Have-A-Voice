using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialMessageWrapper : AbstractMessageModel<Message> {
        public static SocialMessageWrapper Create(Message anExternal) {
            return new SocialMessageWrapper(anExternal);
        }

        public override Message FromModel() {
            return Message.CreateMessage(Id, ToUserId, FromUserId, Subject, Body, DateTimeStamp, ToViewed, FromViewed, ToDeleted, FromDeleted, RepliedTo);
        }

        private SocialMessageWrapper(Message anExternal) {
            Id = anExternal.Id;
            ToUserId = anExternal.ToUserId;
            FromUserId = anExternal.FromUserId;
            Subject = anExternal.Subject;
            Body = anExternal.Body;
            DateTimeStamp = anExternal.DateTimeStamp;
            ToViewed = anExternal.ToViewed;
            FromViewed = anExternal.FromViewed;
            ToDeleted = anExternal.ToDeleted;
            FromDeleted = anExternal.FromDeleted;
            RepliedTo = anExternal.RepliedTo;
            if (anExternal.FromUser != null) {
                FromUserFullName = NameHelper.FullName(anExternal.FromUser);
                FromUserProfilePictureUrl = PhotoHelper.ProfilePicture(anExternal.FromUser);
            }
        }
    }
}