using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Services.Clubs;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.Services.Events;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Services.TextBooks;
using UniversityOfMe.Services.Users;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.Services.Notifications;

namespace UniversityOfMe.Services {
    public class UniversityService : IUniversityService {
        private IUofMeUserService theUserService;
        private IDatingService theDatingService;
        private IProfessorService theProfessorService;
        private IClassService theClassService;
        private IEventService theEventService;
        private ITextBookService theTextBookService;
        private IClubService theClubService;
        private IGeneralPostingService theGeneralPostingService;
        private INotificationService theNotificationService;
        private IUniversityRepository theUniversityRepository;

        public UniversityService(IValidationDictionary aValidationDictionary) 
            : this(new UofMeUserService(aValidationDictionary),
                   new DatingService(),
                   new ProfessorService(aValidationDictionary), 
                   new ClassService(aValidationDictionary), 
                   new EventService(aValidationDictionary), 
                   new TextBookService(aValidationDictionary), 
                   new ClubService(aValidationDictionary), 
                   new GeneralPostingService(aValidationDictionary), 
                   new NotificationService(),
                   new EntityUniversityRepository()) { }

        public UniversityService(IUofMeUserService aUserService,
                                 IDatingService aDatingService,
                                 IProfessorService aProfessorService, 
                                 IClassService aClassService, 
                                 IEventService anEventService, 
                                 ITextBookService aTextBookService, 
                                 IClubService aClubService, 
                                 IGeneralPostingService aGeneralPostingService, 
                                 INotificationService aNotificationService,
                                 IUniversityRepository aUniversityRepository) {
            theUserService = aUserService;
            theDatingService = aDatingService;
            theProfessorService = aProfessorService;
            theClassService = aClassService;
            theEventService = anEventService;
            theTextBookService = aTextBookService;
            theClubService = aClubService;
            theGeneralPostingService = aGeneralPostingService;
            theNotificationService = aNotificationService;
            theUniversityRepository = aUniversityRepository;
        }

        public void AddUserToUniversity(User aUser) {
            string myUniversityEmail = EmailHelper.ExtractEmailExtension(aUser.Email);
            theUniversityRepository.AddUserToUniversity(aUser.Id, myUniversityEmail);
        }

        public IDictionary<string, string> CreateAllUniversitiesDictionaryEntry() {
            IEnumerable<University> myUniversities = theUniversityRepository.Universities();
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add(Constants.SELECT, Constants.SELECT);
            foreach(University myUniversity in myUniversities) {
                myDictionary.Add(myUniversity.UniversityName, myUniversity.Id);
            }
            return myDictionary;
        }

        public IDictionary<string, string> CreateAcademicTermsDictionaryEntry() {
            IEnumerable<AcademicTerm> myAcademicTerms = theUniversityRepository.GetAcademicTerms();
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add(Constants.SELECT, Constants.SELECT);
            foreach (AcademicTerm myAcademicTerm in myAcademicTerms) {
                myDictionary.Add(myAcademicTerm.DisplayName, myAcademicTerm.Id);
            }
            return myDictionary;
        }

        public UniversityView GetUniversityProfile(UserInformationModel<User> aUserInformation, string aUniversityId) {
            University myUniversity = theUniversityRepository.GetUniversity(aUniversityId);
            IEnumerable<Professor> myProfessors = theProfessorService.GetProfessorsForUniversity(aUniversityId);
            IEnumerable<Class> myClasses = theClassService.GetClassesForUniversity(aUniversityId);
            IEnumerable<Event> myEvents = theEventService.GetEventsForUniversity(aUserInformation.Details, aUniversityId);
            IEnumerable<TextBook> myTextBooks = theTextBookService.GetTextBooksForUniversity(aUniversityId);
            IEnumerable<Club> myOrganizations = theClubService.GetClubs(aUniversityId);
            IEnumerable<GeneralPosting> myGeneralPostings = theGeneralPostingService.GetGeneralPostingsForUniversity(aUniversityId);
            IEnumerable<SendItem> mySendItems = theNotificationService.GetSendItemsForUser(aUserInformation.Details);
            IEnumerable<NotificationModel> myNotifications = ConvertToNotificationModel(mySendItems);


            return new UniversityView() {
                University = myUniversity,
                DatingMember = theDatingService.GetDatingMember(aUserInformation.Details),
                DatingMatchMember = theDatingService.UserDatingMatch(aUserInformation.Details),
                Professors = myProfessors,
                Classes = myClasses,
                Events = myEvents,
                TextBooks = myTextBooks,
                Organizations = myOrganizations,
                GeneralPostings = myGeneralPostings,
                NewestUsers = theUserService.GetNewestUsers(aUserInformation.Details, aUniversityId, 10),
                Notifications = myNotifications
            };
        }

        public bool IsValidUniversityEmailAddress(string anEmailAddress) {
            string anEmailExtension = EmailHelper.ExtractEmailExtension(anEmailAddress);
            IEnumerable<string> myValidEmails = theUniversityRepository.ValidEmails();
            return myValidEmails.Contains(anEmailExtension);
        }

        public IEnumerable<string> ValidEmails() {
            return theUniversityRepository.ValidEmails();
        }

        public bool IsFromUniversity(User aUser, string aUniversityId) {
            string myEmailExtension = EmailHelper.ExtractEmailExtension(aUser.Email);
            IEnumerable<string> myUniversityEmails = theUniversityRepository.UniversityEmails(aUniversityId);
            return myUniversityEmails.Contains(myEmailExtension);
        }

        private IEnumerable<NotificationModel> ConvertToNotificationModel(IEnumerable<SendItem> aSendItems) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();

            foreach (SendItem mySendItem in aSendItems) {
                myNotificationModel.Add(new NotificationModel() {
                    WhoSent = mySendItem.FromUser,
                    Url = "whatever.com",
                    DisplayText = "sent you a beer."
                });
            }

            return myNotificationModel;
        }
    }
}