﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVNotificationService : HAVBaseService, IHAVNotificationService {
        private IHAVNotificationRepository theNotificationRepo;

        public HAVNotificationService()
            : this(new HAVBaseRepository(), new EntityHAVNotificationRepository()) { }

        public HAVNotificationService(IHAVBaseRepository aBaseRepository, IHAVNotificationRepository aNotificationRepo)
            : base(aBaseRepository) {
                theNotificationRepo = aNotificationRepo;
        }

        public IEnumerable<NotificationModel> GetNotifications(User aUser) {
            IEnumerable<Board> myUnreadBoards = theNotificationRepo.UnreadBoardMessages(aUser);
            IEnumerable<BoardViewedState> myUnreadParticipatingBoards = theNotificationRepo.UnreadParticipatingBoardMessages(aUser);
            IEnumerable<IssueViewedState> myUnreadIssues = theNotificationRepo.UnreadIssues(aUser);
            IEnumerable<IssueReplyViewedState> myUnreadIssueReplies = theNotificationRepo.UnreadIssueReplies(aUser);
            IEnumerable<IssueReplyViewedState> myUnreadParticipatingIssueReplies = theNotificationRepo.UnreadParticipatingIssueReplies(aUser);

            return CreateModel(myUnreadBoards, myUnreadParticipatingBoards, myUnreadIssues, myUnreadIssueReplies, myUnreadParticipatingIssueReplies);
        }

        private IEnumerable<NotificationModel> CreateModel(IEnumerable<Board> aBoards, IEnumerable<BoardViewedState> aParticipatingBoards, 
                                                           IEnumerable<IssueViewedState> anIssueStates, IEnumerable<IssueReplyViewedState> anIssueReplies, 
                                                           IEnumerable<IssueReplyViewedState> aParticipatingIssueReplies) {
            List<NotificationModel> myNotifications = new List<NotificationModel>();
            foreach (Board myBoard in aBoards) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.Board,
                    Label = myBoard.Message,
                    Id = myBoard.Id.ToString(),
                    TriggeredUser = myBoard.PostedByUser,
                    DateTimeStamp = myBoard.DateTimeStamp
                });
            }

            foreach (BoardViewedState myBoard in aParticipatingBoards) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.ParticipatingBoard,
                    Label = myBoard.Board.Message,
                    Id = myBoard.Board.Id.ToString(),
                    DateTimeStamp = myBoard.DateTimeStamp
                });
            }

            foreach (IssueViewedState myViewState in anIssueStates) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.Issue,
                    Label = myViewState.Issue.Title,
                    Id = IssueTitleHelper.ConvertForUrl(myViewState.Issue.Title),
                    DateTimeStamp = myViewState.LastUpdated
                });
            }

            foreach (IssueReplyViewedState myViewState in anIssueReplies) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.IssueReply,
                    Label = myViewState.IssueReply.Reply,
                    Id = myViewState.IssueReply.Id.ToString(),
                    DateTimeStamp = myViewState.LastUpdated
                });
            }

            foreach (IssueReplyViewedState myViewState in aParticipatingIssueReplies) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.ParticipatingIssueReply,
                    Label = myViewState.IssueReply.Reply,
                    Id = myViewState.IssueReply.Id.ToString(),
                    DateTimeStamp = myViewState.LastUpdated
                });
            }

            return myNotifications;
        }
    }
}