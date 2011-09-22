using System;
using System.Collections.Generic;
using System.Linq;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.Services.Notifications;
using UniversityOfMe.Services.Users;
using UniversityOfMe.Services.Badges;

namespace UniversityOfMe.Models.View {
    public class LoggedInModel {
        public LeftNavigation LeftNavigation { get; set; }
        public University University { get; set; }

        public LoggedInModel(User aUser) {
            if (aUser != null) {
                INotificationService myNotificationService = new NotificationService();
                IUofMeUserService myUserService = new UofMeUserService(new ModelStateWrapper(null));
                IDatingService myDatingService = new DatingService();
                IBadgeService myBadgeService = new BadgeService();

                IEnumerable<NotificationModel> myNotifications = myNotificationService.GetNotificationsForUser(aUser, 5);
                IEnumerable<User> myNewestUsers = myUserService.GetNewestUsers(aUser, UniversityHelper.GetMainUniversityId(aUser), 12);
                UserBadge myUserBadge = myBadgeService.GetLatestUnseenBadgeForUser(aUser);
                Badge myBadge = null;

                if (myUserBadge != null) {
                    myBadge = myBadgeService.GetBadgeByName(myUserBadge.BadgeName);
                }
                University = UniversityHelper.GetMainUniversity(aUser);

                LeftNavigation = new LeftNavigation() {
                    User = aUser,
                    NewestUsersInUniversity = myNewestUsers,
                    Notifications = myNotifications,
                    DatingMember = myDatingService.GetDatingMember(aUser),
                    DatingMatchMember = myDatingService.UserDatingMatch(aUser),
                    LatestUserBadge = myUserBadge,
                    Badge = myBadge,
                    IsLoggedIn = true
                };
            } else {
                LeftNavigation = new LeftNavigation() {
                    IsLoggedIn = false
                };
            }
        }
    }
}