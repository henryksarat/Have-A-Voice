using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
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
            IEnumerable<BoardViewedState> myBoardViewedStates = theNotificationRepository.GetBoardViewedStates(aUser);

            List<NotificationModel> myNotifications = ConvertToNotificationModel(mySentItems);
            myNotifications.AddRange(ConvertToNotificationModel(aUser, myBoardViewedStates));

            if (myNotifications.Count == 0) {
                myNotifications.Add(NoNotifications());
            }

            return myNotifications.OrderByDescending(n => n.DateTimeSent).Take<NotificationModel>(aLimit);
        }

        private IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return theNotificationRepository.GetSendItemsForUser(aUser);
        }

        private NotificationModel NoNotifications() {
            return new NotificationModel() {
                NotificationType = NotificationType.None
            };
        }

        private List<NotificationModel> ConvertToNotificationModel(IEnumerable<SendItem> aSendItems) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();

            foreach (SendItem mySendItem in aSendItems) {
                myNotificationModel.Add(new NotificationModel() {
                    Id = mySendItem.Id,
                    NotificationType = NotificationType.SentItems,
                    SendItem = SendItemOptions.BEER,
                    WhoSent = mySendItem.FromUser,
                    DateTimeSent = mySendItem.DateTimeStamp
                });
            }

            return myNotificationModel;
        }

        private List<NotificationModel> ConvertToNotificationModel(User anOwnerUser, IEnumerable<BoardViewedState> aBoardViewedStates) {
            List<NotificationModel> myNotificationModel = new List<NotificationModel>();

            foreach (BoardViewedState myBoardViewedState in aBoardViewedStates) {
                Board myBoard = myBoardViewedState.Board;

                myNotificationModel.Add(new NotificationModel() {
                    NotificationType = NotificationType.Board,
                    Board = myBoardViewedState.Board,
                    IsMine = myBoard.OwnerUserId == anOwnerUser.Id,
                    DateTimeSent = myBoardViewedState.DateTimeStamp
                });
            }

            return myNotificationModel;
        }
    }
}
