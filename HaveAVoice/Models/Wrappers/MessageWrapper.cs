using System;
using System.ComponentModel.DataAnnotations;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models {
    public class MessageWrapper {
        public int ToUserId { get; set; }

        public string ToFullName { get; set; }

        public string ToUserProfilePictureUrl { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Subject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Body { get; set; }

        public Message ToModel() {
            return Message.CreateMessage(0, ToUserId, 0, Subject, Body, DateTime.UtcNow, false, false, false, false, false);
        }

        public static MessageWrapper Build(User aUser, string aProfilePictureUrl) {
            return new MessageWrapper() {
                ToUserId = aUser.Id,
                ToFullName = NameHelper.FullName(aUser),
                ToUserProfilePictureUrl = aProfilePictureUrl
            };
        }
    }
}