using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models {
    public class MessageWrapper {
        public int ToUserId { get; set; }

        public string ToUserName { get; set; }

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
                ToUserName = aUser.Username,
                ToUserProfilePictureUrl = aProfilePictureUrl
            };
        }
    }
}