using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class MessageBuilder {
        private int theId;
        private int theToUserId;
        private int theFromUserId;
        private String theSubject = string.Empty;
        private String theBody = string.Empty;
        private DateTime theDateTimeStamp = DateTime.Now;
        private bool theToViewed = false;
        private bool theFromViewed = false;
        private bool theToDeleted = false;
        private bool theFromDeleted = false;
        private bool theRepliedTo = false;

        public MessageBuilder(int anId) { 
            theId = anId;
        }

        public MessageBuilder ToUserId(int aToUserId) {
            theToUserId = aToUserId;
            return this;
        }

        public MessageBuilder FromUserId(int aFromUserId) {
            theFromUserId = aFromUserId;
            return this;
        }

        public MessageBuilder Subject(String aSubject) {
            theSubject = aSubject;
            return this;
        }

        public MessageBuilder Body(String aBody) {
            theBody = aBody;
            return this;
        }


        public MessageBuilder DateTimeStamp(DateTime aDateTimeStamp) {
            theDateTimeStamp = aDateTimeStamp;
            return this;
        }

        public MessageBuilder ToViewed(bool aToViewed) {
            theToViewed = aToViewed;
            return this;
        }

        public MessageBuilder FromViewed(bool aFromViewed) {
            theFromViewed = aFromViewed;
            return this;
        }

        public MessageBuilder ToDeleted(bool aToDeleted) {
            theToDeleted = aToDeleted;
            return this;
        }

        public MessageBuilder FromDeleted(bool aFromDeleted) {
            theFromDeleted = aFromDeleted;
            return this;
        }

        public MessageBuilder RepliedTo(bool aRepliedTo) {
            theRepliedTo = aRepliedTo;
            return this;
        }
        public Message Build() {
            return Message.CreateMessage(theId, theToUserId, theFromUserId, theSubject, theBody, theDateTimeStamp, 
                theToViewed, theFromViewed, theToDeleted, theFromDeleted, theRepliedTo);
        }
    }
}