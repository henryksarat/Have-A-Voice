﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVProfileService : HAVBaseService, IHAVProfileService {
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVFriendService theFriendService;
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IHAVBoardRepository theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVUserRetrievalService(), new HAVFriendService(), new EntityHAVProfileRepository(), new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, IHAVUserRetrievalService aUserRetrievalService, IHAVFriendService aFriendService, IHAVProfileRepository aRepository,
                                            IHAVBoardRepository aBoardRepository, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theFriendService = aFriendService;
            theRepository = aRepository;
            theBoardRepository = aBoardRepository;
        }

        public UserProfileModel Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUser(aUserId);
            IEnumerable<Board> myBoardMessages = theBoardRepository.FindBoardByUserId(aUserId);

            UserProfileModel myProfileModel = new UserProfileModel(myUser) {
                BoardFeed = CreateBoardFeed(myBoardMessages),
                IssueFeed = CreateIssueFeed(theRepository.UserIssueFeed(aUserId)),
                IssueReplyFeed = CreateIssueReplyFeed(theRepository.UserIssueReplyFeed(aUserId))
            };

            foreach (Board myBoard in myBoardMessages) {
                theBoardRepository.MarkBoardAsViewed(myViewingUser, myBoard.Id);
            }

            return myProfileModel;
        }

        private IEnumerable<IssueFeedModel> CreateIssueFeed(IEnumerable<Issue> anIssues) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();

            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

                IssueFeedModel myFeedModel = new IssueFeedModel(myIssue.User) {
                    DateTimeStamp = myIssue.DateTimeStamp,
                    Title = myIssue.Title,
                    Description = myIssue.Description,
                    TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueDisposition>(),
                    TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueDisposition>(),
                    HasDisposition = (from d in myIssueDisposition where d.UserId == myIssue.User.Id select d).Count<IssueDisposition>() > 1 ? true : false,
                    TotalReplys = myIssue.IssueReplys.Count,
                    IssueReplys = myIssue.IssueReplys
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueReplyFeedModel> CreateIssueReplyFeed(IEnumerable<IssueReply> anIssueReplys) {
            List<IssueReplyFeedModel> myFeedModels = new List<IssueReplyFeedModel>();

            foreach (IssueReply myIssueReply in anIssueReplys) {
                IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

                IssueReplyFeedModel myFeedModel = new IssueReplyFeedModel(myIssueReply.User) {
                    DateTimeStamp = myIssueReply.DateTimeStamp,
                    IssueReplyComments = myIssueReply.IssueReplyComments,
                    Issue = myIssueReply.Issue,
                    Reply = myIssueReply.Reply,
                    TotalLikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueReplyDisposition>(),
                    TotalDislikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueReplyDisposition>(),
                    HasDisposition = (from d in myReplyDisposition where d.UserId == myIssueReply.User.Id select d).Count<IssueReplyDisposition>() > 1 ? true : false,
                    TotalComments = myIssueReply.IssueReplyComments.Count
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<BoardFeedModel> CreateBoardFeed(IEnumerable<Board> aBoards) {
            List<BoardFeedModel> myFeedModels = new List<BoardFeedModel>();

            foreach (Board myBoard in aBoards) {
                BoardFeedModel myFeedModel = new BoardFeedModel(myBoard.User) {
                    BoardId = myBoard.Id,
                    DateTimeStamp = myBoard.DateTimeStamp,
                    Message = myBoard.Message,
                    BoardReplys = myBoard.BoardReplies
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private FriendStatus GetFriendStatus(int aSourceUserId, User aViewingUser) {
            FriendStatus myFriendStatus = FriendStatus.None;
            bool myIsPending = theFriendService.IsPending(aSourceUserId, aViewingUser);
            bool myIsFriend = false;

            if (!myIsPending) {
                myIsFriend = theFriendService.IsFriend(aSourceUserId, aViewingUser);
                if (myIsFriend) {
                    myFriendStatus = FriendStatus.Approved;
                }
            } else {
                myFriendStatus = FriendStatus.Pending;
            }

            return myFriendStatus;
        }

        public UserProfileModel MyProfile(User aUser) {
            UserProfileModel myModel = new UserProfileModel(aUser) {
                IssueFeed = CreateIssueFeed(theRepository.FriendIssueFeed(aUser)),
                IssueReplyFeed = CreateIssueReplyFeed(theRepository.FriendIssueReplyFeed(aUser))
            };

            return myModel;
        }
    }
}