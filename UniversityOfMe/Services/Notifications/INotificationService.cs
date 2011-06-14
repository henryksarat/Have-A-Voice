using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Notifications {
    public interface INotificationService {
        IEnumerable<NotificationModel> GetNotificationsForUser(User aUser, int aLimit);
    }
}