using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVNotificationService {
        IEnumerable<NotificationModel> GetNotifications(User aUser);
    }
}