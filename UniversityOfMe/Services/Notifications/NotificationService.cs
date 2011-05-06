using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Repositories.Notifications;
using System.Collections.Generic;

namespace UniversityOfMe.Services.Notifications {
    public class NotificationService : INotificationService {
        private INotificationRepository theNotificationRepository;

        public NotificationService()
            : this(new EntityNotificationRepository()) { }

        public NotificationService(INotificationRepository aNotificationRepo) {
            theNotificationRepository = aNotificationRepo;
        }

        public IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return theNotificationRepository.GetSendItemsForUser(aUser);
        }
    }
}
