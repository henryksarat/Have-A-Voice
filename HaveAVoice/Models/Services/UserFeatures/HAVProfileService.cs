using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.Repositories.UserFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Models.Services.UserFeatures {
    public class HAVProfileService : HAVBaseService, IHAVProfileService {
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IHAVBoardRepository theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new EntityHAVProfileRepository(), new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, IHAVProfileRepository aRepository,
                                IHAVBoardRepository aBoardRepository, IHAVBaseRepository aBaseRepository)
            : base(aBaseRepository) {
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
            theBoardRepository = aBoardRepository;
        }

        public ProfileModelBuilder Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = myUserService.GetUser(aUserId);
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