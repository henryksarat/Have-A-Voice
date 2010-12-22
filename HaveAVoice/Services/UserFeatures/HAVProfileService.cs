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
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IHAVBoardRepository theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVUserRetrievalService(), new EntityHAVProfileRepository(), new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, IHAVUserRetrievalService aUserRetrievalService, IHAVProfileRepository aRepository,
                                            IHAVBoardRepository aBoardRepository, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theUserRetrievalService = aUserRetrievalService;
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
            theBoardRepository = aBoardRepository;
        }

        public ProfileModelBuilder Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUser(aUserId);
            IEnumerable<Board> myBoardMessages = theBoardRepository.FindBoardByUserId(aUserId);
            IEnumerable<IssueReply> myIssueReplys = theRepository.IssuesUserRepliedTo(myUser);
            bool myIsFan = theRepository.IsFan(myUser.Id, myViewingUser);
            IEnumerable<Fan> myFans = theRepository.FindFansForUser(myUser.Id);
            IEnumerable<Fan> myFansOf = theRepository.FindFansOfUser(myUser.Id);

            ProfileModelBuilder myModel = new ProfileModelBuilder(myUser)
                .SetBoardMessages(myBoardMessages)
                .SetIsFan(myIsFan)
                .SetIssueReplys(myIssueReplys)
                .SetFans(myFans)
                .SetFansOf(myFansOf);

            return myModel;
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return theRepository.FindFansForUser(aUserId);
        }

         public IEnumerable<Fan> FindFansOfUser(int aUserId) {
             return theRepository.FindFansOfUser(aUserId);
         }
    }
}