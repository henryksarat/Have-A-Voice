using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Repositories.UserFeatures;
using HaveAVoice.Models.Services.UserFeatures;
using HaveAVoice.Models.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HaveAVoice.Tests.Controllers.Users {
    [TestClass]
    public class ComplaintControllerTest : ControllerTestCase {
        private static int COMPLAINT_SOURCE_ID = 3;
        private static string COMPLAINT = "I'm complaining.";
        private static string COMPLAINT_DESCRIPTION = "Complaint description";
        
        private ComplaintModel theModel;
        private Issue theIssue;
        private IHAVComplaintService theService;
        private Mock<IHAVComplaintService> theMockedService;
        private Mock<IHAVUserService> theMockedUserService;
        private Mock<IHAVIssueService> theMockIssueService;
        private Mock<IHAVComplaintRepository> theMockRepository;
        private ComplaintController theController;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVComplaintService>();
            theMockedUserService = new Mock<IHAVUserService>();
            theMockIssueService = new Mock<IHAVIssueService>();
            theMockRepository = new Mock<IHAVComplaintRepository>();

            theService = new HAVComplaintService(new ModelStateWrapper(theModelState),
                                                                   theMockRepository.Object,
                                                                   theBaseRepository.Object);

            theController = new ComplaintController(theMockedService.Object, theMockedBaseService.Object, 
                theMockedUserService.Object, theMockIssueService.Object);
            theController.ControllerContext = GetControllerContext();

            theIssue = Issue.CreateIssue(COMPLAINT_SOURCE_ID, COMPLAINT, COMPLAINT_DESCRIPTION, DateTime.UtcNow, false);
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
