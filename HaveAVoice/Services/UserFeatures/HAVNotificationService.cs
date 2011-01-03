using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVNotificationService : HAVBaseService, IHAVNotificationService {
        private IHAVNotificationRepository theNotificationRepo;

        public HAVNotificationService()
            : this(new HAVBaseRepository(), new EntityHAVNotificationRepository()) { }

        public HAVNotificationService(IHAVBaseRepository aBaseRepository, IHAVNotificationRepository aNotificationRepo)
            : base(aBaseRepository) {
                theNotificationRepo = aNotificationRepo;
        }

        public IEnumerable<Models.Board> GetNotifications(User aUser) {
            return theNotificationRepo.UnreadBoardMessages(aUser);
        }
    }
}