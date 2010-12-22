using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVProfileService : HAVBaseService, IHAVProfileService {
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVFanService theFanService;
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IHAVBoardRepository theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVUserRetrievalService(), new HAVFanService(), new EntityHAVProfileRepository(), new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, IHAVUserRetrievalService aUserRetrievalService, IHAVFanService aFanService, IHAVProfileRepository aRepository,
                                            IHAVBoardRepository aBoardRepository, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theUserRetrievalService = aUserRetrievalService;
            theFanService = aFanService;
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
            theBoardRepository = aBoardRepository;
        }

        public ProfileModelBuilder Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUser(aUserId);
            IEnumerable<Board> myBoardMessages = theBoardRepository.FindBoardByUserId(aUserId);
            IEnumerable<IssueReply> myIssueReplys = theRepository.IssuesUserRepliedTo(myUser);
            bool myIsFan = theFanService.IsFan(myUser.Id, myViewingUser);
            IEnumerable<Fan> myFans = theFanService.FindFansForUser(myUser.Id);
            IEnumerable<Fan> myFansOf = theFanService.FindFansOfUser(myUser.Id);

            ProfileModelBuilder myModel = new ProfileModelBuilder(myUser)
                .SetBoardMessages(myBoardMessages)
                .SetIsFan(myIsFan)
                .SetIssueReplys(myIssueReplys)
                .SetFans(myFans)
                .SetFansOf(myFansOf);

            return myModel;
        }
    }
}