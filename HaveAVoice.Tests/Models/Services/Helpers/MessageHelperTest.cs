using System;
using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaveAVoice.Tests.Services.Helpers {
    [TestClass]
    public class MessageHelperTest {
        /*
        private const int MESSAGE_ID = 3;
        private const int OTHER_MESSAGE_ID = 4;
        private const string MESSAGE_TITLE = "subject1";
        private const string MESSAGE_BODY = "body1";
        private const string OTHER_MESSAGE_TITLE = "subject2";
        private const string OTHER_MESSAGE_BODY = "body2";
        private const string REPLY = "reply1";
        private const string OTHER_REPLY = "reply2";

        private DateTime theMessageDateTime = new DateTime(2010, 11, 29, 5, 5, 36);
        private DateTime theOtherMessageDateTime = new DateTime(2010, 11, 29, 6, 5, 36);
        private DateTime theReplyDateTime = new DateTime(2010, 11, 29, 7, 5, 36);
        private DateTime theOtherReplyDateTime = new DateTime(2010, 11, 29, 8, 5, 36);

        private User theToUser = DatamodelFactory.createUser(1);
        private User theFromUser = DatamodelFactory.createUser(2);
        private Reply theReplyOne;
        private Reply theReplyTwo;

        private MessageBuilder theMessageBuilder = new MessageBuilder(MESSAGE_ID);

        [TestInitialize]
        public void Initialize() {
            theMessageBuilder.DateTimeStamp(theMessageDateTime)
                .ToUserId(theToUser.Id)
                .FromUserId(theFromUser.Id)
                .Subject(MESSAGE_TITLE)
                .Body(MESSAGE_BODY);
            theReplyOne = Reply.CreateReply(1, theToUser.Id, MESSAGE_ID, REPLY, theReplyDateTime);
            theReplyTwo = Reply.CreateReply(2, theFromUser.Id, MESSAGE_ID, OTHER_REPLY, theOtherReplyDateTime);
        }

        [TestMethod]
        public void ShouldDisplayOneMessage() {
            List<Message> myMessages = CreateMessageList();
            List<Reply> myReplies = new List<Reply>();

            List<InboxMessage> myToFromInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myToFromInbox, 1, MESSAGE_BODY);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertEmptyInbox(myFromUserInbox);
        }

        [TestMethod]
        public void ShouldDisplayOneMessageWithLatestReply() {
            List<Message> myMessages = CreateMessageList();

            List<Reply> myReplies = new List<Reply>();
            myReplies.Add(theReplyOne);

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myToUserInbox, 1, REPLY);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myFromUserInbox, 1, REPLY);
        }

        [TestMethod]
        public void ShouldDisplayOneMessageWithLatestReplyFromTwoReplies() {
            List<Message> myMessages = CreateMessageList();

            List<Reply> myReplies = new List<Reply>();
            myReplies.Add(theReplyOne);
            myReplies.Add(theReplyTwo);

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myToUserInbox, 1, OTHER_REPLY);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myFromUserInbox, 1, OTHER_REPLY);
        }

        [TestMethod]
        public void ShouldNotDisplayToUserIfDeletedButFromUserCanStillSeeMessage() {
            theMessageBuilder.ToDeleted(true);
            List<Message> myMessages = CreateMessageList();

            List<Reply> myReplies = new List<Reply>();
            myReplies.Add(theReplyOne);

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            AssertEmptyInbox(myToUserInbox);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myFromUserInbox, 1, REPLY);
        }

        [TestMethod]
        public void ShouldDisplayToUserButShouldNotDisplayForFromUser() {
            theMessageBuilder.FromDeleted(true);
            List<Message> myMessages = CreateMessageList();

            List<Reply> myReplies = new List<Reply>();
            myReplies.Add(theReplyOne);

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myToUserInbox, 1, REPLY);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertEmptyInbox(myFromUserInbox);
        }

        [TestMethod]
        public void ShouldDisplayTwoNewMessagesInOrderForToUserAndNoneForFromUser() {
            List<Message> myMessages = CreateMessageListWithTwoMessages();
            List<Reply> myReplies = new List<Reply>();

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            Assert.AreEqual(2, myToUserInbox.Count);
            Assert.AreEqual(OTHER_MESSAGE_BODY, myToUserInbox[0].LastReply);
            Assert.AreEqual(MESSAGE_BODY, myToUserInbox[1].LastReply);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertEmptyInbox(myFromUserInbox);
        }

        [TestMethod]
        public void ShouldDisplayMessageOneFirstBecauseReplyIsNewer() {
            List<Message> myMessages = CreateMessageListWithTwoMessages();
            List<Reply> myReplies = new List<Reply>();
            myReplies.Add(theReplyOne);

            List<InboxMessage> myToUserInbox = MessageHelper.GenerateInbox(theToUser, myMessages, myReplies);
            Assert.AreEqual(2, myToUserInbox.Count);
            Assert.AreEqual(REPLY, myToUserInbox[0].LastReply);
            Assert.AreEqual(OTHER_MESSAGE_BODY, myToUserInbox[1].LastReply);

            List<InboxMessage> myFromUserInbox = MessageHelper.GenerateInbox(theFromUser, myMessages, myReplies);
            AssertLatestBodyDisplayed(myFromUserInbox, 1, REPLY);
        }

        private List<Message> CreateMessageList() {
            Message myMessage = theMessageBuilder.Build();
            myMessage.ToUser = theToUser;
            myMessage.FromUser = theFromUser;

            List<Message> myMessages = new List<Message>();
            myMessages.Add(myMessage);
            return myMessages;
        }

        private void AssertEmptyInbox(List<InboxMessage> anInbox) {
            Assert.AreEqual(0, anInbox.Count);
        }

        private void AssertLatestBodyDisplayed(List<InboxMessage> anInbox, int anExpectedCount, string anExpectedBody) {
            Assert.AreEqual(anExpectedCount, anInbox.Count);
            Assert.AreEqual(anExpectedBody, anInbox[0].LastReply);
        }

        private List<Message> CreateMessageListWithTwoMessages() {
            Message myMessageOne = theMessageBuilder.Build();
            myMessageOne.ToUser = theToUser;
            myMessageOne.FromUser = theFromUser;
            Message myMessageTwo = new MessageBuilder(OTHER_MESSAGE_ID)
                .ToUserId(theToUser.Id)
                .FromUserId(theFromUser.Id)
                .Subject(OTHER_MESSAGE_TITLE)
                .Body(OTHER_MESSAGE_BODY)
                .DateTimeStamp(theOtherMessageDateTime)
                .Build();
            myMessageTwo.ToUser = theToUser;
            myMessageTwo.FromUser = theFromUser;

            List<Message> myMessages = new List<Message>();
            myMessages.Add(myMessageOne);
            myMessages.Add(myMessageTwo);

            return myMessages;
        }
         * */

    }
}
