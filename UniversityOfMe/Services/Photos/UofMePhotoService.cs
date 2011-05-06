using HaveAVoice.Services.UserFeatures;
using Social.Friend.Services;
using Social.Photo.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.Repositories.Photos;
using Social.Validation;
using UniversityOfMe.Models.View;
using Social.Generic.Models;
using UniversityOfMe.Models.Social;
using System.Collections;
using System.Collections.Generic;

namespace UniversityOfMe.Services.Photos {
    public class UofMePhotoService : PhotoService<User, PhotoAlbum, Photo, Friend>, IUofMePhotoService {
        private IUofMePhotoRepository thePhotoRepo;

        public UofMePhotoService()
            : this(new FriendService<User, Friend>(new EntityFriendRepository()), new EntityPhotoAlbumRepository(), new EntityPhotoRepository()) { }

        public UofMePhotoService(IFriendService<User, Friend> aFriendService, IPhotoAlbumRepository<User, PhotoAlbum, Photo> aPhotoAlbumRepo, IUofMePhotoRepository aPhotoRepo)
            : base(aFriendService, aPhotoAlbumRepo, aPhotoRepo) {
            thePhotoRepo = aPhotoRepo;
        }

        public PhotoDisplayView GetPhotoDisplayView(User aUserInformation, int aPhotoId) {
            Photo myPhoto = GetPhoto(SocialUserModel.Create(aUserInformation), aPhotoId).Model;
            IEnumerable<Photo> myOtherPhotos = thePhotoRepo.GetOtherPhotosInPhotosAlbum(myPhoto);
            Photo myPreviousPhoto = null;
            Photo myNextPhoto = null;
            bool myIsCurrentPhoto = false;

            foreach (Photo myOtherPhoto in myOtherPhotos) {
                if (!myIsCurrentPhoto && myOtherPhoto.Id != myPhoto.Id) {
                    myPreviousPhoto = myOtherPhoto;
                } else if (myOtherPhoto.Id == myPhoto.Id) {
                    myIsCurrentPhoto = true;
                } else if (myIsCurrentPhoto) {
                    myNextPhoto = myOtherPhoto;
                    break;
                }
            }

            return new PhotoDisplayView() {
                Photo = myPhoto,
                NextPhoto = myNextPhoto,
                PreviousPhoto = myPreviousPhoto
            };
                        
        }
    }
}