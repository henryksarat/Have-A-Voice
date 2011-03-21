using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaveAVoice.Tests.Controllers.Users {
    [TestClass]
    public class ComplaintControllerTest : ControllerTestCase {
        /*
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

        protected override Controller GetController() {
            return theController;
        }
        */
    }
}
