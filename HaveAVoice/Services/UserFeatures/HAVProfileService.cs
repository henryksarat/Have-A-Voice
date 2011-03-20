using System;
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
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using Social.Friend.Services;
using HaveAVoice.Models.SocialWrappers;
using Social.User.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVProfileService : HAVBaseService, IHAVProfileService {
        private IHAVUserRetrievalService theUserRetrievalService;
        private IFriendService<User, Friend> theFriendService;
        private IHAVPhotoAlbumService thePhotoAlbumService;
        private IHAVProfileRepository theRepository;
        private IValidationDictionary theValidationDictionary;
        private IHAVBoardRepository theBoardRepository;

        public HAVProfileService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVUserRetrievalService(), new FriendService<User, Friend>(new EntityHAVFriendRepository()), new HAVPhotoAlbumService(validationDictionary), new EntityHAVProfileRepository(), new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVProfileService(IValidationDictionary aValidationDictionary, IHAVUserRetrievalService aUserRetrievalService, IFriendService<User, Friend> aFriendService, IHAVPhotoAlbumService aPhotoAlbumService, IHAVProfileRepository aRepository,
                                            IHAVBoardRepository aBoardRepository, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
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
            AbstractUserModel<User> myAbstractUser = new UserModel(aViewingUser);

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

        public UserProfileModel AuthorityProfile(UserInformationModel anAuthorityUserInformation) {
            User myAuthorityUser = anAuthorityUserInformation.Details;
            List<IssueFeedModel> myIssueFeed = CreateIssueFeedForAuthority(theRepository.FilteredIssuesFeed(myAuthorityUser), anAuthorityUserInformation, PersonFilter.People).ToList<IssueFeedModel>();
            List<IssueReplyFeedModel> myIssueReplyFeed = CreateIssueReplyFeedForAuthority(theRepository.FilteredIssueReplysFeed(myAuthorityUser), anAuthorityUserInformation, PersonFilter.People).ToList<IssueReplyFeedModel>();
            IEnumerable<IssueFeedModel> myPoliticiansIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticianRoles()), myAuthorityUser, PersonFilter.Politicians);
            IEnumerable<IssueReplyFeedModel> myPoliticiansIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticianRoles()), myAuthorityUser, PersonFilter.Politicians);
            IEnumerable<IssueFeedModel> myPoliticalCandidateIssueFeed = CreateIssueFeed(theRepository.IssueFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), myAuthorityUser, PersonFilter.PoliticalCandidates);
            IEnumerable<IssueReplyFeedModel> myPoliticalCandidateIssueReplyFeed = CreateIssueReplyFeed(theRepository.IssueReplyFeedByRole(UserRoleHelper.PoliticalCandidateRoles()), myAuthorityUser, PersonFilter.PoliticalCandidates);

            myIssueFeed.AddRange(myPoliticiansIssueFeed);
            myIssueFeed.AddRange(myPoliticalCandidateIssueFeed);
            myIssueFeed = myIssueFeed.OrderByDescending(i => i.DateTimeStamp).ToList<IssueFeedModel>();

            myIssueReplyFeed.AddRange(myPoliticiansIssueReplyFeed);
            myIssueReplyFeed.AddRange(myPoliticalCandidateIssueReplyFeed);
            myIssueReplyFeed = myIssueReplyFeed.OrderByDescending(ir => ir.DateTimeStamp).ToList<IssueReplyFeedModel>();

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

        private IEnumerable<IssueFeedModel> CreateIssueFeedForAuthority(IEnumerable<Issue> anIssues, User aViewingUser) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();

            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;
                if (PrivacyHelper.IsAllowed(myIssue.User, PrivacyAction.DisplayProfile)) {

                }
                IssueFeedModel myFeedModel = new IssueFeedModel(myIssue.User) {
                    Id = myIssue.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssue.DateTimeStamp),
                    Title = myIssue.Title,
                    Description = myIssue.Description,
                    State = myIssue.State,
                    City = myIssue.City,
                    TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Like select d).Count<IssueDisposition>(),
                    TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Dislike select d).Count<IssueDisposition>(),
                    HasDisposition = (from d in myIssueDisposition where d.UserId == aViewingUser.Id select d).Count<IssueDisposition>() > 0 ? true : false,
                    TotalReplys = myIssue.IssueReplys.Count,
                    IssueReplys = myIssue.IssueReplys
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueFeedModel> CreateIssueFeed(IEnumerable<Issue> anIssues, User aViewingUser, PersonFilter aPersonFilter) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();

            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

                IssueFeedModel myFeedModel = new IssueFeedModel(myIssue.User) {
                    Id = myIssue.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssue.DateTimeStamp),
                    Title = myIssue.Title,
                    City = myIssue.City,
                    State = myIssue.State,
                    Description = myIssue.Description,
                    PersonFilter = aPersonFilter,
                    TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Like select d).Count<IssueDisposition>(),
                    TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Dislike select d).Count<IssueDisposition>(),
                    HasDisposition = ((aViewingUser == null ) || (from d in myIssueDisposition where d.UserId == aViewingUser.Id select d).Count<IssueDisposition>() > 0) ? true : false,
                    TotalReplys = myIssue.IssueReplys.Count,
                    IssueReplys = myIssue.IssueReplys
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueFeedModel> CreateIssueFeedForAuthority(IEnumerable<Issue> anIssues, UserInformationModel aViewingUser, PersonFilter aPersonFilter) {
            List<IssueFeedModel> myFeedModels = new List<IssueFeedModel>();
            foreach (Issue myIssue in anIssues) {
                IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

                bool myIsAllowed = PrivacyHelper.IsAllowed(myIssue.User, PrivacyAction.DisplayProfile, aViewingUser);
                User myUser = myIssue.User;

                if (!myIsAllowed) {
                    myUser = ProfileHelper.GetAnonymousProfile();
                }

                IssueFeedModel myFeedModel = new IssueFeedModel(myUser) {
                    Id = myIssue.Id,
                    DateTimeStamp = TimezoneHelper.ConvertToLocalTimeZone(myIssue.DateTimeStamp),
                    Title = myIssue.Title,
                    City = myIssue.City,
                    State = myIssue.State,
                    Description = myIssue.Description,
                    PersonFilter = aPersonFilter,
                    TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Like select d).Count<IssueDisposition>(),
                    TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.Dislike select d).Count<IssueDisposition>(),
                    HasDisposition = (from d in myIssueDisposition where d.UserId == aViewingUser.Details.Id select d).Count<IssueDisposition>() > 0 ? true : false,
                    TotalReplys = myIssue.IssueReplys.Count,
                    IssueReplys = myIssue.IssueReplys
                };

                myFeedModels.Add(myFeedModel);
            }

            return myFeedModels;
        }

        private IEnumerable<IssueReplyFeedModel> CreateIssueReplyFeedForAuthority(IEnumerable<IssueReply> anIssueReplys, UserInformationModel aViewingUser, PersonFilter aPersonFilter) {
            List<IssueReplyFeedModel> myFeedModels = new List<IssueReplyFeedModel>();

            foreach (IssueReply myIssueReply in anIssueReplys) {
                IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

                bool myIsAllowed = PrivacyHelper.IsAllowed(myIssueReply.User, PrivacyAction.DisplayProfile, aViewingUser);
                User myUser = myIssueReply.User;

                if (!myIsAllowed) {
                    myUser = ProfileHelper.GetAnonymousProfile();
                }

                IssueReplyFeedModel myFeedModel = new IssueReplyFeedModel(myUser) {
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
            AbstractUserModel<User> myAbstractUserModel = new UserModel(aViewingUser);
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