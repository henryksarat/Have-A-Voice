using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Issues;
using Social.Friend.Services;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVHomeService : HAVBaseService, IHAVHomeService {
        private const int MOST_POPULAR_ISSUES_TO_DISPLAY = 10;
        private const int LATEST_ISSUES_TO_DISPLAY = 10;

        private IValidationDictionary theValidationDictionary;
        private IHAVIssueService theIssueService;
        private IFriendService<User, Friend> theFriendService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVIssueService(aValidationDictionary), new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVIssueService anIssueService, IFriendService<User, Friend> aFriendService, 
                              IHAVHomeRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theIssueService = anIssueService;
            theFriendService = aFriendService;
            theHomeRepository = aRepository;
        }

        public NotLoggedInModel NotLoggedIn() {
                return new NotLoggedInModel() {
                    MostPopular = theIssueService.GetMostPopularIssuesByHitCount(MOST_POPULAR_ISSUES_TO_DISPLAY),
                    Newest = theIssueService.GetNewestIssues(LATEST_ISSUES_TO_DISPLAY)
            };
        }
    }
}
