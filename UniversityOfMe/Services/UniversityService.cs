using System.Collections.Generic;
using System.Linq;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.Services.Clubs;
using UniversityOfMe.Services.Events;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.Services.Notifications;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Services.TextBooks;
using UniversityOfMe.Services.Status;
using UniversityOfMe.Services.Dating;

namespace UniversityOfMe.Services {
    public class UniversityService : IUniversityService {
        private IProfessorService theProfessorService;
        private IFlirtingService theFlirtingService;
        private IClassService theClassService;
        private IEventService theEventService;
        private ITextBookService theTextBookService;
        private IClubService theClubService;
        private IUserStatusService theUserStatusService;
        private IGeneralPostingService theGeneralPostingService;
        private INotificationService theNotificationService;
        private IUniversityRepository theUniversityRepository;

        public UniversityService(IValidationDictionary aValidationDictionary) 
            : this(new ProfessorService(aValidationDictionary), 
                   new ClassService(aValidationDictionary), 
                   new EventService(aValidationDictionary), 
                   new TextBookService(aValidationDictionary), 
                   new ClubService(aValidationDictionary), 
                   new GeneralPostingService(aValidationDictionary), 
                   new NotificationService(),
                   new UserStatusService(aValidationDictionary),
                   new FlirtingService(aValidationDictionary),
                   new EntityUniversityRepository()) { }

        public UniversityService(IProfessorService aProfessorService, 
                                 IClassService aClassService, 
                                 IEventService anEventService, 
                                 ITextBookService aTextBookService, 
                                 IClubService aClubService, 
                                 IGeneralPostingService aGeneralPostingService, 
                                 INotificationService aNotificationService,
                                 IUserStatusService aUserStatusRepo,
                                 IFlirtingService aFlirtingService,
                                 IUniversityRepository aUniversityRepository) {
            theProfessorService = aProfessorService;
            theClassService = aClassService;
            theEventService = anEventService;
            theTextBookService = aTextBookService;
            theClubService = aClubService;
            theGeneralPostingService = aGeneralPostingService;
            theNotificationService = aNotificationService;
            theUniversityRepository = aUniversityRepository;
            theFlirtingService = aFlirtingService;
            theUserStatusService = aUserStatusRepo;
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

        public University GetUniversityById(string aUniversityId) {
            return theUniversityRepository.GetUniversity(aUniversityId);
        }

        public UniversityView GetUniversityProfile(UserInformationModel<User> aUserInformation, string aUniversityId) {
            University myUniversity = theUniversityRepository.GetUniversity(aUniversityId);
            IEnumerable<Professor> myProfessors = theProfessorService.GetProfessorsForUniversity(aUniversityId);
            IEnumerable<Class> myClasses = theClassService.GetClassesForUniversity(aUniversityId);
            IEnumerable<Event> myEvents = theEventService.GetEventsForUniversity(aUserInformation.Details, aUniversityId);
            IEnumerable<TextBook> myTextBooks = theTextBookService.GetTextBooksForUniversity(aUniversityId);
            IEnumerable<Club> myOrganizations = theClubService.GetClubs(aUserInformation, aUniversityId);
            IEnumerable<GeneralPosting> myGeneralPostings = theGeneralPostingService.GetGeneralPostingsForUniversity(aUniversityId);
            IEnumerable<UserStatus> myUserStatuses = theUserStatusService.GetLatestUserStatusesWithinUniversity(aUserInformation, aUniversityId, 5);
            IEnumerable<AnonymousFlirt> myAnonymousFlirt = theFlirtingService.GetFlirtsWithinUniversity(aUniversityId, 5);
            UserStatus myUserStatus = theUserStatusService.GetLatestUserStatusForUser(aUserInformation);

            return new UniversityView() {
                University = myUniversity,
                Professors = myProfessors,
                Classes = myClasses,
                Events = myEvents,
                TextBooks = myTextBooks,
                Organizations = myOrganizations,
                GeneralPostings = myGeneralPostings,
                UserStatuses = myUserStatuses,
                CurrentStatus = myUserStatus,
                AnonymousFlirts = myAnonymousFlirt
            };
        }

        public IEnumerable<University> GetValidUniversities() {
            return theUniversityRepository.ValidUniversities();
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
    }
}