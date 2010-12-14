using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Models;
using HaveAVoice.Models.Repositories.UserFeatures;
using HaveAVoice.Models.View;

namespace HaveAVoice.Tests.Models.Repositories.UserFeatures {
    [TestClass]
    public class EntityHAVMessageRepositoryTest {
        private User theToUser = User.CreateUser(1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new DateTime(), new DateTime(), new DateTime(), string.Empty, false, string.Empty);
        private User theFromUser = User.CreateUser(2, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new DateTime(), new DateTime(), new DateTime(), string.Empty, false, string.Empty);
        private MessageBuilder theMessageBuilder = new MessageBuilder(1).Subject("subject1").Body("body1");
        private Reply theReply1 = Reply.CreateReply(0, string.Empty, new DateTime());
        
        private List<Message> theMessages = new List<Message>();
        private List<Reply> theReplys = new List<Reply>();

        [TestMethod]
        public void TestInbox_MessageSentFromUserToUser() {
            Message myMessage = theMessageBuilder.Build();
            AddMessage(myMessage);

            AssertMessageInboxForUser(theToUser, 1);
            AssertMessageInboxForUser(theFromUser, 0);
        }


        private void AssertMessageInboxForUser(User anUser, int anExpectedMessageCount) {
            List<InboxMessage> myModel = EntityHAVMessageRepository.Helpers.GenerateInbox(anUser, theMessages, theReplys);
  
            Assert.AreEqual(anExpectedMessageCount, myModel.Count);

        }

        [TestMethod]
        public void TestInbox_MessageSentFromUserToUserWithReplyBack() {
            Message myMessage = theMessageBuilder.Build();
            AddMessage(myMessage);
            theReply1.Message = myMessage;
            theReplys.Add(theReply1);

            AssertMessageInboxForUser(theToUser, 1);
            AssertMessageInboxForUser(theFromUser, 1);
        }

        [TestMethod]
        public void TestInbox_MessageSentFromUserToUserWithReplyBack_BothDeleted() {
            Message myMessage = theMessageBuilder.Build();
            AddMessage(myMessage);
            theReply1.Message = myMessage;
            theReplys.Add(theReply1);

            AssertMessageInboxForUser(theToUser, 1);
            AssertMessageInboxForUser(theFromUser, 1);
            myMessage = theMessageBuilder.ToDeleted(true).Build();
            AddMessage(myMessage);
            theMessages.Clear();
            theMessages.Add(myMessage);
            AssertMessageInboxForUser(theToUser, 0);
            AssertMessageInboxForUser(theFromUser, 1);
            myMessage = theMessageBuilder.ToDeleted(true).FromDeleted(true).Build();
            AddMessage(myMessage);
            theMessages.Clear();
            theMessages.Add(myMessage);
            AssertMessageInboxForUser(theToUser, 0);
            AssertMessageInboxForUser(theFromUser, 0);
        }



        private void AddMessage(Message aMessage) {
            aMessage.ToUser = theToUser;
            aMessage.FromUser = theFromUser;
            theMessages.Add(aMessage);
        }
    }
}
