using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using Social.Board.Repositories;
using Social.Friend.Services;
using Social.Generic.Models;
using Social.Photo.Services;
using Social.User.Services;
using Social.Validation;
using HaveAVoice.Helpers.Authority;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVProfileService : IHAVProfileService {
        private IUserRetrievalService<User> theUserRetrievalService;
        private IFriendService<User, Friend> theFriendService;
        private IPhotoAlbumService<User, PhotoAlbum, Photo, Friend> thePhotoAlbumService;
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IBoardRepository<User, Board, BoardReply> theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary,
                   new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository()), 
                   new FriendService<User, Friend>(new EntityHAVFriendRepository()),
                   new PhotoAlbumService<User, PhotoAlbum, Photo, Friend>(validationDictionary, 
                       new PhotoService<User, PhotoAlbum, Photo, Friend>(new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository()),
                       new FriendService<User, Friend>(new EntityHAVFriendRepository()),
                       new EntityHAVPhotoAlbumRepository()),
                   new EntityHAVProfileRepository(),
                   new EntityHAVBoardRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, 
                                 IUserRetrievalService<User> aUserRetrievalService, 
                                 IFriendService<User, Friend> aFriendService, 
                                 IPhotoAlbumService<User, PhotoAlbum, Photo, Friend> aPhotoAlbumService, 
                                 IHAVProfileRepository aRepository,
                                 IBoardRepository<User, Board, BoardReply> aBoardRepository) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theFriendService = aFriendService;
            theRepository = aRepository;
            theBoardRepository = aBoardRepository;
            thePhotoAlbumService = aPhotoAlbumService;
        }

        public UserProfileModel Profile(int aUserId, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUser(aUserId);
            return Profile(myUser, myViewingUser);
        }

        public UserProfileModel Profile(string aShortUrl, User myViewingUser) {
            IHAVUserService myUserService = new HAVUserService(theValidationDictionary);
            User myUser = theUserRetrievalService.GetUserByShortUrl(aShortUrl);
            if(myUser == null) {
                return null;
            }
            return Profile(myUser, myViewingUser);
        }

        public UserProfileModel MyProfile(User aUser) {
            List<IssueFeedModel> myIssueFeed = CreateIssueFeed(theRepository.FriendIssueFeed(aUser), aUser, PersonFilter.People).ToList<IssueFeedModel>();
            List<IssueReplyFeedModel> myIssueReplyFeed = CreateIssueReplyFeed(theRepository.FriendIssueReplyFeed(aUser), aUser, PersonFilter.People).ToList<IssueReplyFeedModel>();
            IEnumerable<IssueFeedModel> myPoliticiansIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticianRoles()), aUser, PersonFilter.Politicians);
            IEnumerable<IssueReplyFeedModel> myPoliticiansIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticianRoles()), aUser, PersonFilter.Politicians);
            IEnumerable<IssueFeedModel> myPoliticalCandidateIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), aUser, PersonFilter.PoliticalCandidates);
            IEnumerable<IssueReplyFeedModel> myPoliticalCandidateIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), aUser, PersonFilter.PoliticalCandidates);

            myIssueFeed.AddRange(myPoliticiansIssueFeed);
            myIssueFeed.AddRange(myPoliticalCandidateIssueFeed);
            myIssueFeed = myIssueFeed.OrderByDescending(i => i.DateTimeStamp).ToList<IssueFeedModel>();

            myIssueReplyFeed.AddRange(myPoliticiansIssueReplyFeed);
            myIssueReplyFeed.AddRange(myPoliticalCandidateIssueReplyFeed);
            myIssueReplyFeed = myIssueReplyFeed.OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReplyFeedModel>();

            Issue myRandomLocalIssue = theRepository.RandomLocalIssue(aUser);

            UserProfileModel myModel = new UserProfileModel(aUser) {
                LocalIssue = myRandomLocalIssue,
                IssueFeed = myIssueFeed,
                IssueReplyFeed = myIssueReplyFeed,
                //PhotoAlbumFeed = CreatePhotoAlbumFeed(theRepository.FriendPhotoAlbumFeed(aUser))
            };

            return myModel;
        }

        public UserProfileModel UserIssueActivity(int aUserId, User aViewingUser) {
            AbstractUserModel<User> myAbstractUser = SocialUserModel.Create(aViewingUser);

            if (theFriendService.IsFriend(aUserId, myAbstractUser)) {
                User myUser = theUserRetrievalService.GetUser(aUserId);
                UserProfileModel myModel = new UserProfileModel(myUser) {
                    IssueFeed = CreateIssueFeed(theRepository.IssuesUserCreated(myUser), aViewingUser, PersonFilter.People),
                    IssueReplyFeed = CreateIssueReplyFeed(theRepository.IssuesUserRepliedTo(myUser), aViewingUser, PersonFilter.People)
                };

                return myModel;
            }

            throw new CustomException(HAVConstants.NOT_ALLOWED);
        }

        public UserProfileModel AuthorityProfile(UserInformationModel<User> anAuthorityUserInformation) {
            User myAuthorityUser = anAuthorityUserInformation.Details;
            IEnumerable<Issue> myPeoplesIssues = new List<Issue>();
            IEnumerable<IssueReply> myPeoplesIssueReplies = new List<IssueReply>();

            string myAuthorityPosition = myAuthorityUser.UserPosition.Position.ToUpper();

            if (AuthorityClassification.GetAuthorityPostionsViewableByZip().Contains(myAuthorityPosition)) {
                myPeoplesIssues = theRepository.AuthorityIssuesFeedByZipCode(myAuthorityUser);
                myPeoplesIssueReplies = theRepository.AuthorityIssueReplysFeedByZipCode(myAuthorityUser);
            } else if (AuthorityClassification.GetAuthorityPostionsViewableByCityState().Contains(myAuthorityPosition)) {
                myPeoplesIssues = theRepository.AuthorityIssuesFeedByCityState(myAuthorityUser);
                myPeoplesIssueReplies = theRepository.AuthorityIssueReplysFeedByCityState(myAuthorityUser);
            } else if (AuthorityClassification.GetAuthorityPostionsViewableByState().Contains(myAuthorityPosition)) {
                myPeoplesIssues = theRepository.AuthorityIssuesFeedByState(myAuthorityUser);
                myPeoplesIssueReplies = theRepository.AuthorityIssueReplysFeedByState(myAuthorityUser);
            }

            List<IssueFeedModel> myIssueFeed = CreateIssueFeedForAuthority(myPeoplesIssues, anAuthorityUserInformation, PersonFilter.People).ToList<IssueFeedModel>();
            List<IssueReplyFeedModel> myIssueReplyFeed = CreateIssueReplyFeedForAuthority(myPeoplesIssueReplies, anAuthorityUserInformation, PersonFilter.People).ToList<IssueReplyFeedModel>();
            IEnumerable<IssueFeedModel> myPoliticiansIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticianRoles()), myAuthorityUser, PersonFilter.Politicians);
            IEnumerable<IssueReplyFeedModel> myPoliticiansIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticianRoles()), myAuthorityUser, PersonFilter.Politicians);
            IEnumerable<IssueFeedModel> myPoliticalCandidateIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), myAuthorityUser, PersonFilter.PoliticalCandidates);
            IEnumerable<IssueReplyFeedModel> myPoliticalCandidateIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), myAuthorityUser, PersonFilter.PoliticalCandidates);

            myIssueFeed.AddRange(myPoliticiansIssueFeed);
            myIssueFeed.AddRange(myPoliticalCandidateIssueFeed);
            myIssueFeed = myIssueFeed.OrderByDescending(i => i.DateTimeStamp).Take<IssueFeedModel>(10).ToList<IssueFeedModel>();

            myIssueReplyFeed.AddRange(myPoliticiansIssueReplyFeed);
            myIssueReplyFeed.AddRange(myPoliticalCandidateIssueReplyFeed);
            myIssueReplyFeed = myIssueReplyFeed.OrderByDescending(ir => ir.DateTimeStamp).Take<IssueReplyFeedModel>(10).ToList<IssueReplyFeedModel>();

            Issue myRandomLocalIssue = theRepository.RandomLocalIssue(myAuthorityUser);

            UserProfileModel myModel = new UserProfileModel(myAuthorityUser) {
                LocalIssue = myRandomLocalIssue,
                IssueFeed = myIssueFeed,
                IssueReplyFeed = myIssueReplyFeed,
            };

            return myModel;
        }

        private UserProfileModel Profile(User aUser, User aViewingUser) {
            IEnumerable<Board> myBoardMessages = theBoardRepository.FindBoardByUserId(aUser.Id);
            //IEnumerable<PhotoAlbum> myPhotoAlbums = thePhotoAlbumService.GetPhotoAlbumsForUser(aViewingUser, aUser.Id);

            UserProfileModel myProfileModel = new UserProfileModel(aUser) {
                BoardFeed = CreateBoardFeed(myBoardMessages),
                IssueFeed = CreateIssueFeed(theRepository.UserIssueFeed(aUser.Id), aViewingUser, PersonFilter.People),
                IssueReplyFeed = CreateIssueReplyFeed(theRepository.UserIssueReplyFeed(aUser.Id), aViewingUser, PersonFilter.People),
            };

            if (aViewingUser != null) {
                foreach (Board myBoard in myBoardMessages) {
                    theBoardRepository.MarkBoardAsViewed(aViewingUser, myBoard.Id);
                }
            }

            return myProfileModel;
        }

        private IEnumerable<IssueFeedModel> CreateIssueFeed(IEnumerable<Issue> anIssues, User aViewingUser, PersonFilter aPersonFilter) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();

            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

                IssueFeedModel myFeedModel = new IssueFeedModel(myIssue.User) {
                    Issue = myIssue,
                    PersonFilter = aPersonFilter,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssue.DateTimeStamp)
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueFeedModel> CreateIssueFeedForAuthority(IEnumerable<Issue> anIssues, UserInformationModel<User> aViewingUser, PersonFilter aPersonFilter) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();
            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

                bool myIsAllowed = PrivacyHelper.IsAllowed(myIssue.User, PrivacyAction.DisplayProfile, aViewingUser);

                IssueFeedModel myFeedModel = new IssueFeedModel(myIssue.User) {
                    Issue = myIssue,
                    PersonFilter = aPersonFilter,
                    IsAnonymous = !myIsAllowed,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssue.DateTimeStamp)
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueReplyFeedModel> CreateIssueReplyFeedForAuthority(IEnumerable<IssueReply> anIssueReplys, UserInformationModel<User> aViewingUser, PersonFilter aPersonFilter) {
            List<IssueReplyFeedModel> myFeedModels = new List<IssueReplyFeedModel>();

            foreach (IssueReply myIssueReply in anIssueReplys) {
                IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

                bool myIsAllowed = PrivacyHelper.IsAllowed(myIssueReply.User, PrivacyAction.DisplayProfile, aViewingUser);
                User myUser = myIssueReply.User;

                if (!myIsAllowed) {
                    myUser = ProfileHelper.GetAnonymousProfile();
                }

                IssueReplyFeedModel myFeedModel = new IssueReplyFeedModel(myUser) {
                    IsAnonymous = !myIsAllowed,
                    Id = myIssueReply.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssueReply.DateTimeStamp),
                    State = myIssueReply.State,
                    City = myIssueReply.City,
                    IssueReplyComments = myIssueReply.IssueReplyComments,
                    Issue = myIssueReply.Issue,
                    Reply = myIssueReply.Reply,
                    PersonFilter = aPersonFilter,
                    TotalLikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.Like select d).Count<IssueReplyDisposition>(),
                    TotalDislikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.Dislike select d).Count<IssueReplyDisposition>(),
                    HasDisposition = (from d in myReplyDisposition where d.UserId == aViewingUser.Details.Id select d).Count<IssueReplyDisposition>() > 0 ? true : false,
                    TotalComments = myIssueReply.IssueReplyComments.Count
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }


        private IEnumerable<IssueReplyFeedModel> CreateIssueReplyFeed(IEnumerable<IssueReply> anIssueReplys, User aViewingUser, PersonFilter aPersonFilter) {
            List<IssueReplyFeedModel> myFeedModels = new List<IssueReplyFeedModel>();

            foreach (IssueReply myIssueReply in anIssueReplys) {
                IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

                IssueReplyFeedModel myFeedModel = new IssueReplyFeedModel(myIssueReply.User) {
                    Id = myIssueReply.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssueReply.DateTimeStamp),
                    State = myIssueReply.State,
                    City = myIssueReply.City,
                    IssueReplyComments = myIssueReply.IssueReplyComments,
                    Issue = myIssueReply.Issue,
                    Reply = myIssueReply.Reply,
                    PersonFilter = aPersonFilter,
                    TotalLikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.Like select d).Count<IssueReplyDisposition>(),
                    TotalDislikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.Dislike select d).Count<IssueReplyDisposition>(),
                    HasDisposition = ((aViewingUser == null) || (from d in myReplyDisposition where d.UserId == aViewingUser.Id select d).Count<IssueReplyDisposition>() > 0) ? true : false,
                    TotalComments = myIssueReply.IssueReplyComments.Count
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<BoardFeedModel> CreateBoardFeed(IEnumerable<Board> aBoards) {
            List<BoardFeedModel> myFeedModels = new List<BoardFeedModel>();

            foreach (Board myBoard in aBoards) {
                BoardFeedModel myFeedModel = new BoardFeedModel(myBoard.PostedByUser) {
                    Id = myBoard.Id,
                    OwnerUserId = myBoard.OwnerUserId,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myBoard.DateTimeStamp),
                    Message = myBoard.Message,
                    BoardReplys = myBoard.BoardReplies
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }
        
        private IEnumerable<PhotoAlbumFeedModel> CreatePhotoAlbumFeed(IEnumerable<PhotoAlbum> aPhotoAlbums) {
            List<PhotoAlbumFeedModel> myFeedModels = new List<PhotoAlbumFeedModel>();

            
            foreach (PhotoAlbum myAlbum in aPhotoAlbums) {
                IEnumerable<Photo> myPhotos = myAlbum.Photos.OrderByDescending(p => p.DateTimeStamp);

                PhotoAlbumFeedModel myFeedModel = new PhotoAlbumFeedModel(myAlbum.User) {
                    Id = myAlbum.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone((from p in myPhotos select p.DateTimeStamp).FirstOrDefault<DateTime>()),
                    Photos = myPhotos,
                    Name = myAlbum.Name,
                    Description = myAlbum.Description
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private FriendStatus GetFriendStatus(int aSourceUserId, User aViewingUser) {
            FriendStatus myFriendStatus = FriendStatus.None;
            AbstractUserModel<User> myAbstractUserModel = SocialUserModel.Create(aViewingUser);
            bool myIsPending = theFriendService.IsPending(aSourceUserId, myAbstractUserModel);
            bool myIsFriend = false;

            if (!myIsPending) {
                myIsFriend = theFriendService.IsFriend(aSourceUserId, myAbstractUserModel);
                if (myIsFriend) {
                    myFriendStatus = FriendStatus.Approved;
                }
            } else {
                myFriendStatus = FriendStatus.Pending;
            }

            return myFriendStatus;
        }

    }
}