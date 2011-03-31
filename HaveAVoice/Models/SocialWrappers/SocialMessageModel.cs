using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialMessageModel : AbstractMessageModel<Message> {
        public static SocialMessageModel Create(Message anExternal) {
            return new SocialMessageModel(anExternal);
        }

        public SocialMessageModel() {
        }

        public override Message FromModel() {
            return Message.CreateMessage(Id, ToUserId, FromUserId, Subject, Body, DateTimeStamp, ToViewed, FromViewed, ToDeleted, FromDeleted, RepliedTo);
        }

        private SocialMessageModel(Message anExternal) {
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
            if (anExternal.ToUser != null) {
                ToUserFullName = NameHelper.FullName(anExternal.ToUser);
                ToUserProfilePictureUrl = PhotoHelper.ProfilePicture(anExternal.ToUser);
            }
        }
    }
}