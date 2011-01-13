using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HaveAVoice.Tests.Controllers.Users {
    [TestClass]
    public class ComplaintControllerTest : ControllerTestCase {
        private const int COMPLAINT_SOURCE_ID = 3;
        private const int STARTED_BY_USER_ID = 0;
        private const string COMPLAINT = "I'm complaining.";
        private const string COMPLAINT_DESCRIPTION = "Complaint description";
        
        private ComplaintModel theModel;
        private Issue theIssue;
        private IHAVComplaintService theService;
        private Mock<IHAVComplaintService> theMockedService;
        private Mock<IHAVIssueService> theMockIssueService;
        private Mock<IHAVPhotoService> theMockPhotoService;
        private Mock<IHAVComplaintRepository> theMockRepository;
        private Mock<IHAVUserRetrievalService> theUserRetrievalService;
        private ComplaintController theController;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVComplaintService>();
            theMockIssueService = new Mock<IHAVIssueService>();
            theMockPhotoService = new Mock<IHAVPhotoService>();
            theMockRepository = new Mock<IHAVComplaintRepository>();
            theUserRetrievalService = new Mock<IHAVUserRetrievalService>();

            theService = new HAVComplaintService(new ModelStateWrapper(theModelState),
                                                                   theMockRepository.Object,
                                                                   theBaseRepository.Object);

            theController = new ComplaintController(theMockedService.Object, theMockedBaseService.Object,
                theUserRetrievalService.Object, theMockIssueService.Object, theMockPhotoService.Object);
            theController.ControllerContext = GetControllerContext();

            theIssue = Issue.CreateIssue(COMPLAINT_SOURCE_ID, COMPLAINT, COMPLAINT_DESCRIPTION, "Chicago", "IL", DateTime.UtcNow, STARTED_BY_USER_ID, false);
        }

        #region "Complaint - NonPostBack"

        [TestMethod]
        public void TestIssueComplaintNonPostBack_Success() {
            theMockIssueService.Setup(i => i.GetIssue(COMPLAINT_SOURCE_ID)).Returns(() => theIssue);

            theModel = new ComplaintModel.Builder(COMPLAINT_SOURCE_ID, ComplaintType.Issue).Build();

            var myResult = theController.Complaint(ComplaintType.Issue.ToString(), COMPLAINT_SOURCE_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Complaint", theModel);
        }

        #endregion


        #region "Complaint - After Post Back"

        [TestMethod]
        public void TestIssueComplaint_Success() {
            theMockedService.Setup(s => s.IssueComplaint(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<int>())).Returns(() => true);
            theModel = new ComplaintModel.Builder(COMPLAINT_SOURCE_ID, ComplaintType.Issue).Build();

            var myResult = theController.Complaint(theModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

      [TestMethod]
      public void TestIssueReplyComplaint_Success() {
          theMockedService.Setup(s => s.IssueReplyComplaint(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<int>())).Returns(() => true);
          theModel = new ComplaintModel.Builder(COMPLAINT_SOURCE_ID, ComplaintType.IssueReply).Build();

          var myResult = theController.Complaint(theModel) as ViewResult;

          AssertAuthenticatedRedirection(myResult);
      }
      
      [TestMethod]
      public void TestIssueReplyCommentComplaint_Success() {
          theMockedService.Setup(s => s.IssueReplyCommentComplaint(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<int>())).Returns(() => true);
          theModel = new ComplaintModel.Builder(COMPLAINT_SOURCE_ID, ComplaintType.IssueReplyComment).Build();

          var myResult = theController.Complaint(theModel) as ViewResult;

          AssertAuthenticatedRedirection(myResult);
      }
               
        [TestMethod]
        public void TestIssueReplyProfileComplaint_Success() {
            theMockedService.Setup(s => s.ProfileComplaint(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<int>())).Returns(() => true);
            theModel = new ComplaintModel.Builder(COMPLAINT_SOURCE_ID, ComplaintType.ProfileComplaint).Build();

            var myResult = theController.Complaint(theModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        #endregion

        protected override Controller GetController() {
            return theController;
        }
    }
}
