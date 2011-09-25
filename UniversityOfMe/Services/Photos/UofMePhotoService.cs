using HaveAVoice.Services.UserFeatures;
using Social.Friend.Services;
using Social.Photo.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.Repositories.Photos;
using Social.Validation;
using UniversityOfMe.Models.View;
using Social.Generic.Models;
using UniversityOfMe.Models.SocialModels;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Amazon.S3;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Services.Photos {
    public class UofMePhotoService : PhotoService<User, PhotoAlbum, Photo, Friend>, IUofMePhotoService {
        private IUofMePhotoRepository thePhotoRepo;

        public UofMePhotoService()
            : this(new FriendService<User, Friend>(new EntityFriendRepository()), new EntityPhotoAlbumRepository(), new EntityPhotoRepository()) { }

        public UofMePhotoService(IFriendService<User, Friend> aFriendService, IPhotoAlbumRepository<User, PhotoAlbum, Photo> aPhotoAlbumRepo, IUofMePhotoRepository aPhotoRepo)
            : base(aFriendService, aPhotoAlbumRepo, aPhotoRepo) {
            thePhotoRepo = aPhotoRepo;
        }

        public bool HasProfilePhoto(User aUser) {
            Photo myPhoto = thePhotoRepo.GetProfilePicture(aUser);
            return myPhoto != null ? true : false;
        }

        new public Photo UploadImageWithDatabaseReference(AbstractUserModel<User> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile, AmazonS3 aAmazonS3Client, string aBucketName, int aMaxSize) {
            Photo myUploadedPhoto = base.UploadImageWithDatabaseReference(aUserToUploadFor, anAlbumId, aImageFile, aAmazonS3Client, aBucketName, aMaxSize);
            if (!thePhotoRepo.HasAlbumCoverAlready(myUploadedPhoto.PhotoAlbumId)) {
                thePhotoRepo.SetPhotoAsAlbumCover(myUploadedPhoto.Id);
            }
            return myUploadedPhoto;
        }


        public PhotoDisplayView GetPhotoDisplayView(User aUserInformation, int aPhotoId) {
            Photo myPhoto = GetPhoto(SocialUserModel.Create(aUserInformation), aPhotoId).Model;
            IEnumerable<Photo> myOtherPhotos = thePhotoRepo.GetOtherPhotosInPhotosAlbum(myPhoto);
            Photo myPreviousPhoto = null;
            Photo myNextPhoto = null;
            bool myIsCurrentPhoto = false;

            foreach (Photo myOtherPhoto in myOtherPhotos.Where(p => !p.ProfilePicture)) {
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

        //Throws custom exception if it's not a jpg, gif, or png image
        public void UploadProfilePicture(UserInformationModel<User> aUser, HttpPostedFileBase anImageFile, AmazonS3 anAmazonS3Client, string aBucketName, int aMaxSize) {
            PhotoAlbum myPhotoAlbum = thePhotoRepo.GetDefaultPhotoAlbumForProfilePictures(aUser.Details, UOMConstants.DEFAULT_PROFILE_PHOTO_ALBUM);
            
            if (myPhotoAlbum == null) {
                myPhotoAlbum = GetPhotoAlbumRepo().Create(aUser.Details, UOMConstants.DEFAULT_PROFILE_PHOTO_ALBUM, string.Empty);
            }
            
            Photo myPhoto = UploadImageWithDatabaseReference(SocialUserModel.Create(aUser.Details), myPhotoAlbum.Id, anImageFile, anAmazonS3Client, aBucketName, aMaxSize);
            SetToProfilePicture(SocialUserModel.Create(aUser.Details), myPhoto.Id, anAmazonS3Client, aBucketName, aMaxSize);
        }
    }
}