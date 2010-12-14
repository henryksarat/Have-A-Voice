using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Repositories.UserFeatures;

namespace HaveAVoice.Tests.Models.Repositories.UserFeatures {

    [TestClass]
    public class EntityHAVHomeRepositoryTest {
        #region "Fields"
        private Issue theIssue1 = Issue.CreateIssue(1, string.Empty, string.Empty, new DateTime(), false);
        private Issue theIssue2 = Issue.CreateIssue(2, string.Empty, string.Empty, new DateTime(), false);
        private Issue theIssue3 = Issue.CreateIssue(3, string.Empty, string.Empty, new DateTime(), false);
        private IssueReply theIssueReply1 = IssueReply.CreateIssueReply(1, string.Empty, new DateTime(), false, (int)Disposition.LIKE, false);
        private IssueReply theIssueReply2 = IssueReply.CreateIssueReply(2, string.Empty, new DateTime(), false, (int)Disposition.LIKE, false);
        private IssueReply theIssueReply3 = IssueReply.CreateIssueReply(3, string.Empty, new DateTime(), false, (int)Disposition.LIKE, false);
        private IssueDisposition theIssueDisposition1 = IssueDisposition.CreateIssueDisposition(1, (int)Disposition.LIKE);
        private IssueDisposition theIssueDisposition2 = IssueDisposition.CreateIssueDisposition(2, (int)Disposition.LIKE);
        private IssueReplyComment theIssueReplyComment1 = IssueReplyComment.CreateIssueReplyComment(1, 0, string.Empty, new DateTime(),0,  false);
        private IssueReplyComment theIssueReplyComment2 = IssueReplyComment.CreateIssueReplyComment(2, 0, string.Empty, new DateTime(), 0, false);
        private IssueReplyComment theIssueReplyComment3 = IssueReplyComment.CreateIssueReplyComment(3, 0, string.Empty, new DateTime(), 0, false);
        private IssueReplyDisposition theIssueReplyDisposition1 = IssueReplyDisposition.CreateIssueReplyDisposition(1, (int)Disposition.LIKE);
        private IssueReplyDisposition theIssueReplyDisposition2 = IssueReplyDisposition.CreateIssueReplyDisposition(2, (int)Disposition.LIKE);
        private IssueReplyDisposition theIssueReplyDisposition3 = IssueReplyDisposition.CreateIssueReplyDisposition(3, (int)Disposition.LIKE);

        List<Issue> theIssues = new List<Issue>();
        List<IssueReply> theIssueReplys = new List<IssueReply>();
        List<IssueDisposition> theIssueDisposition = new List<IssueDisposition>();
        List<IssueReplyComment> theIssueReplyComments = new List<IssueReplyComment>();
        List<IssueReplyDisposition> theIssueReplyDisposition = new List<IssueReplyDisposition>();
        #endregion

        [TestMethod]
        public void TestMostPopular_IssueReplysOnOneIssue() {
            theIssues.Add(theIssue1);
            theIssues.Add(theIssue2);
            theIssues.Add(theIssue3);

            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);

            theIssueDisposition.Add(theIssueDisposition1);
            theIssueDisposition.Add(theIssueDisposition2);

            theIssueReply1.Issue = theIssue2;
            theIssueReply2.Issue = theIssue2;
            theIssueReply3.Issue = theIssue2;

            theIssueDisposition1.Issue = theIssue1;
            theIssueDisposition2.Issue = theIssue1;

            List<IssueWithDispositionModel> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssues, theIssueReplys, theIssueDisposition, Disposition.LIKE).ToList<IssueWithDispositionModel>();

            Assert.AreEqual(2, myModel.Count);
            Assert.AreEqual(theIssue2, myModel[0].Issue);
            Assert.AreEqual(theIssue1, myModel[1].Issue);
        }


        [TestMethod]
        public void TestMostPopular_IssueDispositionVolumeBeatsIssueReply() {
            theIssues.Add(theIssue1);
            theIssues.Add(theIssue2);
            theIssues.Add(theIssue3);
            theIssueReplys.Add(theIssueReply1);

            IssueDisposition myIssueDisposition3 = IssueDisposition.CreateIssueDisposition(3, (int)Disposition.LIKE);
            IssueDisposition myIssueDisposition4 = IssueDisposition.CreateIssueDisposition(4, (int)Disposition.LIKE);
            IssueDisposition myIssueDisposition5 = IssueDisposition.CreateIssueDisposition(5, (int)Disposition.LIKE);

            theIssueDisposition.Add(theIssueDisposition1);
            theIssueDisposition.Add(theIssueDisposition2);
            theIssueDisposition.Add(myIssueDisposition3);
            theIssueDisposition.Add(myIssueDisposition4);
            theIssueDisposition.Add(myIssueDisposition5);        

            theIssueReply1.Issue = theIssue1;

            theIssueDisposition1.Issue = theIssue2;
            theIssueDisposition2.Issue = theIssue2;
            myIssueDisposition3.Issue = theIssue2;
            myIssueDisposition4.Issue = theIssue2;
            myIssueDisposition5.Issue = theIssue2;

            List<IssueWithDispositionModel> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssues, theIssueReplys, theIssueDisposition, Disposition.LIKE).ToList<IssueWithDispositionModel>();

            Assert.AreEqual(2, myModel.Count);
            Assert.AreEqual(theIssue2, myModel[0].Issue);
            Assert.AreEqual(theIssue1, myModel[1].Issue);

        }

        [TestMethod]
        public void TestMostPopular_OppositeDisposition() {
            theIssues.Add(theIssue1);
            theIssues.Add(theIssue2);
            theIssues.Add(theIssue3);

            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);

            theIssueDisposition.Add(theIssueDisposition1);
            theIssueDisposition.Add(theIssueDisposition2);

            theIssueReply1.Issue = theIssue2;
            theIssueReply2.Issue = theIssue2;
            theIssueReply3.Issue = theIssue2;

            theIssueDisposition1.Issue = theIssue1;
            theIssueDisposition2.Issue = theIssue1;

            List<IssueWithDispositionModel> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssues, theIssueReplys, theIssueDisposition, Disposition.DISLIKE).ToList<IssueWithDispositionModel>();


            Assert.AreEqual(0, myModel.Count);
        }

        [TestMethod]
        public void TestMostPopular_OppositeDispositionExceptForOne() {
            theIssues.Add(theIssue1);
            theIssues.Add(theIssue2);
            theIssues.Add(theIssue3);

            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);

            theIssueDisposition.Add(theIssueDisposition1);
            theIssueDisposition.Add(theIssueDisposition2);

            theIssueReply1.Issue = theIssue2;
            theIssueReply2.Issue = theIssue2;
            theIssueReply3.Issue = theIssue2;

            theIssueDisposition1.Issue = theIssue1;
            theIssueDisposition2.Issue = theIssue1;

            theIssueDisposition1.Issue = theIssue3;
            theIssueDisposition1.Disposition = (int)Disposition.DISLIKE;

            List<IssueWithDispositionModel> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssues, theIssueReplys, theIssueDisposition, Disposition.DISLIKE).ToList<IssueWithDispositionModel>();

            Assert.AreEqual(1, myModel.Count);
            Assert.AreEqual(theIssue3, myModel[0].Issue);
        }

        [TestMethod]
        public void TestMostPopularIssueReply_CommentsAndDisposition() {
            theIssueReply1.Issue = theIssue1;
            theIssueReply2.Issue = theIssue1;
            theIssueReply3.Issue = theIssue1;
            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);
            theIssueReplyComments.Add(theIssueReplyComment1);
            theIssueReplyComments.Add(theIssueReplyComment2);
            theIssueReplyDisposition.Add(theIssueReplyDisposition1);

            theIssueReplyComment1.IssueReply = theIssueReply2;
            theIssueReplyComment2.IssueReply = theIssueReply3;
            theIssueReplyDisposition1.IssueReply = theIssueReply2;

            List<IssueReply> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssueReplys, theIssueReplyComments, theIssueReplyDisposition).ToList<IssueReply>();

            Assert.AreEqual(3, myModel.Count);
            Assert.AreEqual(theIssueReply2, myModel[0]);
            Assert.AreEqual(theIssueReply3, myModel[1]);
            Assert.AreEqual(theIssueReply1, myModel[2]);
        }

        [TestMethod]
        public void TestMostPopularIssueReply_Comments() {
            theIssueReply1.Issue = theIssue1;
            theIssueReply2.Issue = theIssue1;
            theIssueReply3.Issue = theIssue1;
            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);
            theIssueReplyComments.Add(theIssueReplyComment1);
            theIssueReplyComments.Add(theIssueReplyComment2);
            theIssueReplyComments.Add(theIssueReplyComment3);

            theIssueReplyComment1.IssueReply = theIssueReply2;
            theIssueReplyComment2.IssueReply = theIssueReply2;
            theIssueReplyComment3.IssueReply = theIssueReply1;

            List<IssueReply> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssueReplys, theIssueReplyComments, theIssueReplyDisposition).ToList<IssueReply>();

            Assert.AreEqual(3, myModel.Count);
            Assert.AreEqual(theIssueReply2, myModel[0]);
            Assert.AreEqual(theIssueReply1, myModel[1]);
            Assert.AreEqual(theIssueReply3, myModel[2]);
        }

        [TestMethod]
        public void TestMostPopularIssueReply_Disposition() {
            theIssueReply1.Issue = theIssue1;
            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReply2.Issue = theIssue1;
            theIssueReplys.Add(theIssueReply3);
            theIssueReply3.Issue = theIssue1;
            theIssueReplyDisposition.Add(theIssueReplyDisposition1);
            theIssueReplyDisposition.Add(theIssueReplyDisposition2);
            theIssueReplyDisposition.Add(theIssueReplyDisposition3);

            theIssueReplyDisposition1.IssueReply = theIssueReply2;
            theIssueReplyDisposition2.IssueReply = theIssueReply2;
            theIssueReplyDisposition3.IssueReply = theIssueReply1;

            List<IssueReply> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssueReplys, theIssueReplyComments, theIssueReplyDisposition).ToList<IssueReply>();

            Assert.AreEqual(3, myModel.Count);
            Assert.AreEqual(theIssueReply2, myModel[0]);
            Assert.AreEqual(theIssueReply1, myModel[1]);
            Assert.AreEqual(theIssueReply3, myModel[2]);
        }

        [TestMethod]
        public void TestMostPopularIssueReply_DispositionWeightBeatsComments() {
            theIssueReply1.Issue = theIssue1;
            theIssueReply2.Issue = theIssue1;
            theIssueReply3.Issue = theIssue1;
            theIssueReplys.Add(theIssueReply1);
            theIssueReplys.Add(theIssueReply2);
            theIssueReplys.Add(theIssueReply3);
            theIssueReplyComments.Add(theIssueReplyComment1);

            IssueReplyDisposition myIssueReplyDisposition4 = IssueReplyDisposition.CreateIssueReplyDisposition(2, (int)Disposition.LIKE);
            IssueReplyDisposition myIssueReplyDisposition5 = IssueReplyDisposition.CreateIssueReplyDisposition(3, (int)Disposition.LIKE);
            theIssueReplyDisposition.Add(theIssueReplyDisposition1);
            theIssueReplyDisposition.Add(theIssueReplyDisposition2);
            theIssueReplyDisposition.Add(theIssueReplyDisposition3);
            theIssueReplyDisposition.Add(myIssueReplyDisposition4);
            theIssueReplyDisposition.Add(myIssueReplyDisposition5);

            theIssueReplyComment1.IssueReply = theIssueReply3;

            theIssueReplyDisposition1.IssueReply = theIssueReply2;
            theIssueReplyDisposition2.IssueReply = theIssueReply2;
            theIssueReplyDisposition3.IssueReply = theIssueReply2;
            myIssueReplyDisposition4.IssueReply = theIssueReply2;
            myIssueReplyDisposition5.IssueReply = theIssueReply2;

            List<IssueReply> myModel =
                EntityHAVHomeRepository.Helpers.CalculatedWithWeights(theIssueReplys, theIssueReplyComments, theIssueReplyDisposition).ToList<IssueReply>();

            Assert.AreEqual(3, myModel.Count);
            Assert.AreEqual(theIssueReply2, myModel[0]);
            Assert.AreEqual(theIssueReply3, myModel[1]);
            Assert.AreEqual(theIssueReply1, myModel[2]);
        }
    }
}
