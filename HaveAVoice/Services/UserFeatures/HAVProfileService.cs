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
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Services.Helpers;

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
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theFanService = aFanService;
            theRepository = aRepository;
            theBoardRepository = aBoardRepository;
        }

        public ProfileModel Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUser(aUserId);
            IEnumerable<Board> myBoardMessages = theBoardRepository.FindBoardByUserId(aUserId);
            IEnumerable<IssueReply> myIssueReplys = theRepository.IssuesUserRepliedTo(myUser);
            IEnumerable<Fan> myFans = theFanService.FindFansForUser(myUser.Id);
            FanStatus myFanStatus = GetFanStatus(aUserId, myViewingUser);

            foreach (Board myBoard in myBoardMessages) {
                theBoardRepository.MarkBoardAsViewed(myViewingUser, myBoard.Id);
            }

            ProfileModel myModel = new ProfileModel(myUser) {
                ProfilePictureUrl = ProfilePictureHelper.ProfilePicture(myUser),
                BoardMessages = myBoardMessages,
                IssueReplys = myIssueReplys,
                Fans = myFans,
                FanStatus = myFanStatus
            };

            return myModel;
        }

        private FanStatus GetFanStatus(int aSourceUserId, User aViewingUser) {
            FanStatus myFanStatus = FanStatus.None;
            bool myIsPending = theFanService.IsPending(aSourceUserId, aViewingUser);
            bool myIsFan = false;

            if (!myIsPending) {
                myIsFan = theFanService.IsFan(aSourceUserId, aViewingUser);
                if (myIsFan) {
                    myFanStatus = FanStatus.Approved;
                }
            } else {
                myFanStatus = FanStatus.Pending;
            }

            return myFanStatus;
        }
    }
}