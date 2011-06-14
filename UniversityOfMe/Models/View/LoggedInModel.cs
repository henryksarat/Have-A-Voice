using System;
using System.Collections.Generic;
using System.Linq;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.Services.Notifications;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Models.View {
    public class LoggedInModel {
        public LeftNavigation LeftNavigation { get; set; }
        public University University { get; set; }

        public LoggedInModel(User aUser) {
            INotificationService myNotificationService = new NotificationService();
            IUofMeUserService myUserService = new UofMeUserService(new ModelStateWrapper(null));
            IDatingService myDatingService = new DatingService();

            IEnumerable<NotificationModel> myNotifications = myNotificationService.GetNotificationsForUser(aUser, 5);
            IEnumerable<User> myNewestUsers = myUserService.GetNewestUsers(aUser, UniversityHelper.GetMainUniversityId(aUser), 10);

            University = UniversityHelper.GetMainUniversity(aUser);

            LeftNavigation = new LeftNavigation() {
                User = aUser,
                NewestUsersInUniversity = myNewestUsers,
                Notifications = myNotifications,
                DatingMember = myDatingService.GetDatingMember(aUser),
                DatingMatchMember = myDatingService.UserDatingMatch(aUser)
            };
        }


    }
}