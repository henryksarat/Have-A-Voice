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

            IEnumerable<SendItem> mySentItems = myNotificationService.GetSendItemsForUser(aUser);
            IEnumerable<ClubMember> myPendingClubMembers = myNotificationService.GetPendingClubMembersOfAdminedClubs(aUser);

            List<NotificationModel> myNotifications = ConvertToNotificationModel(mySentItems);
            myNotifications.AddRange(ConvertToNotificationModel(myPendingClubMembers));

            IEnumerable<User> myNewestUsers = myUserService.GetNewestUsers(aUser, UniversityHelper.GetMainUniversityId(aUser), 10);

            University = UniversityHelper.GetMainUniversity(aUser);

            myNotifications = myNotifications.OrderByDescending(n => n.DateTimeSent).ToList<NotificationModel>(); ;

            LeftNavigation = new LeftNavigation() {
                User = aUser,
                NewestUsersInUniversity = myNewestUsers,
                Notifications = myNotifications,
                DatingMember = myDatingService.GetDatingMember(aUser),
                DatingMatchMember = myDatingService.UserDatingMatch(aUser)
            };
        }

        private List<NotificationModel> ConvertToNotificationModel(IEnumerable<SendItem> aSendItems) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();
            TimeZone localZone = TimeZone.CurrentTimeZone;

            foreach (SendItem mySendItem in aSendItems) {
                myNotificationModel.Add(new NotificationModel() {
                    NotificationType = NotificationType.SentItems,
                    SendItem = SendItemOptions.BEER,
                    WhoSent = mySendItem.FromUser,
                    DateTimeSent = mySendItem.DateTimeStamp
                });
            }

            return myNotificationModel;
        }

        private List<NotificationModel> ConvertToNotificationModel(IEnumerable<ClubMember> aClubMembers) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();
            TimeZone localZone = TimeZone.CurrentTimeZone;

            foreach (ClubMember myClubMember in aClubMembers) {
                myNotificationModel.Add(new NotificationModel() {
                    NotificationType = NotificationType.Club,
                    Club = myClubMember.Club,
                    ClubMemberUser = myClubMember.ClubMemberUser,
                    DateTimeSent = myClubMember.DateTimeStamp
                });
            }

            return myNotificationModel;
        }
    }
}