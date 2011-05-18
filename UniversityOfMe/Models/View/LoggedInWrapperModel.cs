using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.BaseWebsite.Models;
using UniversityOfMe.Services.Notifications;
using Social.Validation;
using UniversityOfMe.Services.Users;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Dating;
using System;

namespace UniversityOfMe.Models.View {
    public class LoggedInWrapperModel<T> : ILoggedInModel<T> {
        public User User { get; private set; }
        public LeftNavigation LeftNavigation { get; set; }

        private T Model;

        public LoggedInWrapperModel(User aUser) {
            User = aUser;

            INotificationService myNotificationService = new NotificationService();
            IUofMeUserService myUserService = new UofMeUserService(new ModelStateWrapper(null));
            IDatingService myDatingService = new DatingService();

            IEnumerable<NotificationModel> myNotifications = ConvertToNotificationModel(myNotificationService.GetSendItemsForUser(aUser));
            IEnumerable<User> myNewestUsers = myUserService.GetNewestUsers(aUser, UniversityHelper.GetMainUniversity(aUser), 10);

            LeftNavigation = new LeftNavigation() {
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

        public T Get() {
            return Model;
        }

        public void Set(T aModel) {
            Model = aModel;
        }

        public int GetUnreadMessageCount() {
            int myUnreadReceived = (from m in User.MessagesReceived
                                    where m.ToViewed == false
                                    select m).Count<Message>();
            int myUnreadSent = (from m in User.MessagesSent
                                where m.FromViewed == false
                                && m.FromDeleted == false
                                && m.RepliedTo == true
                                select m).Count<Message>();

            return myUnreadReceived + myUnreadSent;
        }
    }
}