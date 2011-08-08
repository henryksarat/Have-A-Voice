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
    public class HAVNotificationService : IHAVNotificationService {
        private IHAVNotificationRepository theNotificationRepo;

        public HAVNotificationService()
            : this(new EntityHAVNotificationRepository()) { }

        public HAVNotificationService(IHAVNotificationRepository aNotificationRepo) {
                theNotificationRepo = aNotificationRepo;
        }

        public int GetNotificationCount(User aUser) {
            int myUnreadBoards = theNotificationRepo.UnreadBoardMessages(aUser).Count<Board>();
            int myUnreadParticipatingBoards = theNotificationRepo.UnreadParticipatingBoardMessages(aUser).Count<BoardViewedState>();
            int myUnreadIssues = theNotificationRepo.UnreadIssues(aUser).Count<IssueViewedState>();
            int myUnreadIssueReplies = theNotificationRepo.UnreadIssueReplies(aUser).Count<IssueReplyViewedState>();
            int myUnreadParticipatingIssueReplies = theNotificationRepo.UnreadParticipatingIssueReplies(aUser).Count<IssueReplyViewedState>();
            int myUnreadGroupMemberBoardPostings = theNotificationRepo.UnreadGroupBoardPosts(aUser).Count<GroupMember>();
            int myUnapprovedGroupMembers = theNotificationRepo.UnapprovedGroupMembers(aUser).Count<GroupMember>();
            int myGroupInvitations = theNotificationRepo.GroupInvitations(aUser).Count<GroupInvitation>();

            return myUnreadBoards + myUnreadParticipatingBoards 
                + myUnreadIssues + myUnreadIssueReplies
                + myUnreadParticipatingIssueReplies + myUnreadGroupMemberBoardPostings
                + myUnapprovedGroupMembers + myGroupInvitations;
        }

        public IEnumerable<NotificationModel> GetNotifications(User aUser) {
            IEnumerable<Board> myUnreadBoards = theNotificationRepo.UnreadBoardMessages(aUser);
            IEnumerable<BoardViewedState> myUnreadParticipatingBoards = theNotificationRepo.UnreadParticipatingBoardMessages(aUser);
            IEnumerable<IssueViewedState> myUnreadIssues = theNotificationRepo.UnreadIssues(aUser);
            IEnumerable<IssueReplyViewedState> myUnreadIssueReplies = theNotificationRepo.UnreadIssueReplies(aUser);
            IEnumerable<IssueReplyViewedState> myUnreadParticipatingIssueReplies = theNotificationRepo.UnreadParticipatingIssueReplies(aUser);
            IEnumerable<GroupMember> myUnreadGroupMemberBoardPostings = theNotificationRepo.UnreadGroupBoardPosts(aUser);
            IEnumerable<GroupMember> myUnapprovedGroupMembers = theNotificationRepo.UnapprovedGroupMembers(aUser);
            IEnumerable<GroupInvitation> myGroupInvitations = theNotificationRepo.GroupInvitations(aUser);

            return CreateModel(myUnreadBoards, myUnreadParticipatingBoards, 
                myUnreadIssues, myUnreadIssueReplies, 
                myUnreadParticipatingIssueReplies, myUnreadGroupMemberBoardPostings,
                myUnapprovedGroupMembers, myGroupInvitations);
        }

        private IEnumerable<NotificationModel> CreateModel(IEnumerable<Board> aBoards, IEnumerable<BoardViewedState> aParticipatingBoards, 
                                                           IEnumerable<IssueViewedState> anIssueStates, IEnumerable<IssueReplyViewedState> anIssueReplies, 
                                                           IEnumerable<IssueReplyViewedState> aParticipatingIssueReplies,
                                                           IEnumerable<GroupMember> aGroupMemberBoardPosts, IEnumerable<GroupMember> aGroupMembersNotApproved,
                                                           IEnumerable<GroupInvitation> aGroupInvitations) {
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

            foreach (GroupMember myViewState in aGroupMemberBoardPosts) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.GroupBoardPost,
                    Label = myViewState.Group.Name,
                    Id = myViewState.GroupId.ToString(),
                    DateTimeStamp = (DateTime)myViewState.LastBoardPost
                });
            }

            foreach (GroupMember myViewState in aGroupMembersNotApproved) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.UnapprovedGroupMember,
                    Label = myViewState.Group.Name,
                    Id = myViewState.Id.ToString(),
                    SecondaryId = myViewState.GroupId.ToString(),
                    DateTimeStamp = (DateTime)myViewState.DateTimeStamp,
                    TriggeredUser = myViewState.MemberUser
                });
            }

            foreach (GroupInvitation myViewState in aGroupInvitations) {
                myNotifications.Add(new NotificationModel() {
                    NotificationType = NotificationType.GroupInvitation,
                    Label = myViewState.Group.Name,
                    Id = myViewState.Id.ToString(),
                    SecondaryId = myViewState.GroupId.ToString(),
                    DateTimeStamp = (DateTime)myViewState.DateTimeStamp,
                    TriggeredUser = myViewState.InvitedByUser
                });
            }

            return myNotifications.OrderByDescending(n => n.DateTimeStamp);
        }
    }
}