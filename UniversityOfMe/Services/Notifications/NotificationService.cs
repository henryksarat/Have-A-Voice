using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Repositories.Notifications;
using System.Collections.Generic;
using UniversityOfMe.Models.View;
using System;
using UniversityOfMe.Helpers;
using System.Linq;

namespace UniversityOfMe.Services.Notifications {
    public class NotificationService : INotificationService {
        private INotificationRepository theNotificationRepository;

        public NotificationService()
            : this(new EntityNotificationRepository()) { }

        public NotificationService(INotificationRepository aNotificationRepo) {
            theNotificationRepository = aNotificationRepo;
        }

        public IEnumerable<NotificationModel> GetNotificationsForUser(User aUser, int aLimit) {
            IEnumerable<SendItem> mySentItems = GetSendItemsForUser(aUser);
            IEnumerable<ClubMember> myPendingClubMembers = GetPendingClubMembersOfAdminedClubs(aUser);
            IEnumerable<BoardViewedState> myBoardViewedStates = theNotificationRepository.GetBoardViewedStates(aUser);


            List<NotificationModel> myNotifications = ConvertToNotificationModel(mySentItems);
            myNotifications.AddRange(ConvertToNotificationModel(myPendingClubMembers));
            myNotifications.AddRange(ConvertToNotificationModel(aUser, myBoardViewedStates));

            if (myNotifications.Count == 0) {
                myNotifications.Add(NoNotifications());
            }

            return myNotifications.OrderByDescending(n => n.DateTimeSent).Take<NotificationModel>(aLimit);
        }

        private IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return theNotificationRepository.GetSendItemsForUser(aUser);
        }

        private IEnumerable<ClubMember> GetPendingClubMembersOfAdminedClubs(User aUser) {
            return theNotificationRepository.GetPendingClubMembersOfAdminedClubs(aUser);
        }

        private NotificationModel NoNotifications() {
            return new NotificationModel() {
                NotificationType = NotificationType.None
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

        private List<NotificationModel> ConvertToNotificationModel(User anOwnerUser, IEnumerable<BoardViewedState> aBoardViewedStates) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();
            TimeZone localZone = TimeZone.CurrentTimeZone;

            foreach (BoardViewedState myBoardViewedState in aBoardViewedStates) {
                Board myBoard = myBoardViewedState.Board;

                myNotificationModel.Add(new NotificationModel() {
                    NotificationType = NotificationType.Board,
                    Board = myBoardViewedState.Board,
                    IsMyBoard = myBoard.OwnerUserId == anOwnerUser.Id,
                    DateTimeSent = myBoardViewedState.DateTimeStamp
                });
            }

            return myNotificationModel;
        }
    }
}
