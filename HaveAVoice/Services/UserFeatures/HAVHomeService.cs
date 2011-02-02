using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System;
using HaveAVoice.Models;
using System.Linq;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVHomeService : HAVBaseService, IHAVHomeService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFriendService theFriendService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFriendService(), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVFriendService aFriendService, 
                              IHAVHomeRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theFriendService = aFriendService;
            theHomeRepository = aRepository;
        }

        public NotLoggedInModel NotLoggedIn() {
                return new NotLoggedInModel() {
                    LikedIssues = theHomeRepository.GetMostPopularIssues(Disposition.Like),
                    DislikedIssues = theHomeRepository.GetMostPopularIssues(Disposition.Dislike),
                    NewestIssueReplys = theHomeRepository.NewestIssueReplys(),
                    MostPopularIssueReplys = theHomeRepository.GetMostPopularIssueReplys()
            };
        }
    }
}
