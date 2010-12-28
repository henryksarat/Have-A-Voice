using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVNavigationService : IHAVNavigationService {
        private IHAVNavigationRepository theNavRepo;

        public HAVNavigationService()
            : this(new EntityHAVNavigationRepository()) { }

        public HAVNavigationService(IHAVNavigationRepository aNavigationRepository) {
            theNavRepo = aNavigationRepository;
        }

        public int NewMessageCount(User aRequestingUser) {
            int myTotalUnreadReceived = theNavRepo.TotalUnreadMessagesReceived(aRequestingUser);
            int myTotalUnreadSent = theNavRepo.TotalUnreadMessagesSent(aRequestingUser);

            return myTotalUnreadReceived + myTotalUnreadSent;
        }
    }
}