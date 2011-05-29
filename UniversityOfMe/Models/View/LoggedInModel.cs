using System;
using System.Collections.Generic;
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

            IEnumerable<NotificationModel> myNotifications = ConvertToNotificationModel(myNotificationService.GetSendItemsForUser(aUser));
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

        private IEnumerable<NotificationModel> ConvertToNotificationModel(IEnumerable<SendItem> aSendItems) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();
            TimeZone localZone = TimeZone.CurrentTimeZone;

            foreach (SendItem mySendItem in aSendItems) {
                myNotificationModel.Add(new NotificationModel() {
                    SendItem = SendItemOptions.BEER,
                    WhoSent = mySendItem.FromUser,
                    FormattedDateTimeSent = DateHelper.ToLocalTime(mySendItem.DateTimeStamp)
                });
            }

            return myNotificationModel;
        }
    }
}