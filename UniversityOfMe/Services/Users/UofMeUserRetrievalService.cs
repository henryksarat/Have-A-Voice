using System.Collections.Generic;
using Social.Generic.Constants;
using Social.User.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Models.View;
using System.Linq;

namespace UniversityOfMe.Services.Users {
    public class UofMeUserRetrievalService : UserRetrievalService<User>, IUofMeUserRetrievalService {
        private IUofMeUserRetrievalRepository theUserRetrievalRepository;

        public UofMeUserRetrievalService()
            : this(new EntityUserRetrievalRepository()) { }

        public UofMeUserRetrievalService(IUofMeUserRetrievalRepository aUserRetrievalRepository)
            : base(aUserRetrievalRepository) {
            theUserRetrievalRepository = aUserRetrievalRepository;
        }

        public IEnumerable<User> GetAllFemaleUsers() {
            return theUserRetrievalRepository.GetUsersWithGender(Gender.FEMALE);
        }

        public IEnumerable<User> GetAllMaleUsers() {
            return theUserRetrievalRepository.GetUsersWithGender(Gender.MALE);
        }

        public User GetUserByChangeEmailInformation(string anOldEmail, string aNewEmailHash) {
            return theUserRetrievalRepository.GetUserByChangeEmailInformation(anOldEmail, aNewEmailHash);
        }


        public ProfileModel GetProfileModelByUserId(int aUserId, bool aShowAllBoards, bool aShowAllAlbums) {
            User myUser = GetUser(aUserId);
            return CreateProfileModel(myUser, aShowAllBoards, aShowAllAlbums);
        }

        public ProfileModel GetProfileModelByShortUrl(string aShortUrl, bool aShowAllBoards, bool aShowAllAlbums) {
            User myUser = GetUserByShortUrl(aShortUrl);
            return CreateProfileModel(myUser, aShowAllBoards, aShowAllAlbums);
        }

        private ProfileModel CreateProfileModel(User aUser, bool aShowAllBoards, bool aShowAllAlbums) {
            IEnumerable<Board> myBoards = new List<Board>();
            int myBoardCount = 0;
            if (aUser != null) {
                if (aShowAllBoards) {
                    myBoards = aUser.Boards.Where(b => !b.Deleted).OrderByDescending(b => b.DateTimeStamp);
                } else {
                    myBoards = aUser.Boards.Where(b => !b.Deleted).OrderByDescending(b => b.DateTimeStamp).Take<Board>(5);
                }

                myBoardCount = aUser.Boards.Where(b => !b.Deleted).OrderByDescending(b => b.DateTimeStamp).Count<Board>();
            }

            IEnumerable<PhotoAlbum> myPhotoAlbums = new List<PhotoAlbum>();
            int myPhotoAlbumCount = 0;
            if (aUser != null) {
                if (aShowAllAlbums) {
                    myPhotoAlbums = aUser.PhotoAlbums.OrderByDescending(b => b.Name);
                } else {
                    myPhotoAlbums = aUser.PhotoAlbums.OrderByDescending(b => b.Name);
                }
                myPhotoAlbumCount = aUser.PhotoAlbums.OrderByDescending(b => b.Name).Count<PhotoAlbum>();
            }

            IEnumerable<UserStatus> myUserStatuses = new List<UserStatus>();
            if (aUser != null) {
                myUserStatuses = aUser.UserStatuses
                    .Where<UserStatus>(us => !us.Deleted)
                    .OrderByDescending(us => us.DateTimeStamp)
                    .Take<UserStatus>(5);
            }

            return new ProfileModel() {
                User = aUser,
                Boards = myBoards,
                BoardCount = myBoardCount,
                ShowAllBoards = aShowAllBoards,
                PhotoAlbums = myPhotoAlbums,
                PhotoAlbumCount = myPhotoAlbumCount,
                ShowAllPhotoAlbums = aShowAllAlbums,
                UserStatuses = myUserStatuses
            };
        }
    }
}
