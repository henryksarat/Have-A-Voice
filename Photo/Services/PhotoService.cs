using System.Collections.Generic;
using System.Web;
using Social.Friend.Exceptions;
using Social.Friend.Services;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Photo.Helpers;
using Social.Photo.Repositories;
using Social.Photo.Services;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    //T = User
    //U = PhotoAlbum
    //V = Photo
    //W = Friend
    public class PhotoService<T, U, V, W> : IPhotoService<T, U, V, W> {
        private const int MAX_SIZE = 840;
        private const string UNAUTHORIZED_UPLOAD = "You are not allowed to upload to that album.";

        private IFriendService<T, W> theFriendService;
        private IPhotoAlbumRepository<T, U, V> thePhotoAlbumRepo;
        private IPhotoRepository<T, V> thePhotoRepo;

        public PhotoService(IFriendService<T, W> aFriendService, IPhotoAlbumRepository<T, U, V> aPhotoAlbumRepo, IPhotoRepository<T, V> aPhotoRepo) {
            thePhotoAlbumRepo = aPhotoAlbumRepo;
            theFriendService = aFriendService;
            thePhotoRepo = aPhotoRepo;
        }

        public IEnumerable<V> GetPhotos(AbstractUserModel<T> aViewingUser, int anAlbumId, int aUserId) {
            if (theFriendService.IsFriend(aUserId, aViewingUser)) {
                return thePhotoRepo.GetPhotos(aUserId, anAlbumId);
            }

            throw new NotFriendException();
        }

        public void DeletePhoto(AbstractUserModel<T> aUserDeleting, int aPhotoId) {
            AbstractPhotoModel<V> myPhoto = GetPhoto(aUserDeleting, aPhotoId);
            if (myPhoto.UploadedByUserId == aUserDeleting.Id) {
                SocialPhotoHelper.PhysicallyDeletePhoto(HttpContext.Current.Server.MapPath(SocialPhotoHelper.ConstructUrl(myPhoto.ImageName)));

                thePhotoRepo.DeletePhoto(aPhotoId);
            } else {
                throw new CustomException(ErrorKeys.PERMISSION_DENIED);
            }
        }

        public AbstractPhotoModel<V> GetPhoto(AbstractUserModel<T> aViewingUser, int aPhotoId) {
            AbstractPhotoModel<V> myPhoto = thePhotoRepo.GetAbstractPhoto(aPhotoId);
            if (theFriendService.IsFriend(myPhoto.UploadedByUserId, aViewingUser)) {
                return myPhoto;
            }

            throw new NotFriendException();
        }

        public void SetToProfilePicture(AbstractUserModel<T> aUser, int aPhotoId) {
            AbstractPhotoModel<V> myCurrentProfile = thePhotoRepo.GetAbstractProfilePicture(aUser.Id);
            if (myCurrentProfile != null) {
                if (myCurrentProfile.UploadedByUserId == aUser.Id) {
                    thePhotoRepo.DeletePhoto(myCurrentProfile.Id);
                    SocialPhotoHelper.PhysicallyDeletePhoto(HttpContext.Current.Server.MapPath(SocialPhotoHelper.ConstructUrl(myCurrentProfile.ImageName)));
                } else {
                    throw new CustomException(ErrorKeys.PERMISSION_DENIED);
                }
            }

            AbstractPhotoModel<V> myNewProfilePhoto = thePhotoRepo.GetAbstractPhoto(aPhotoId);

            if (myNewProfilePhoto.UploadedByUserId == aUser.Id) {
                string[] myNewProfileSplit = myNewProfilePhoto.ImageName.Split('.');
                string myNewProfilePictureImageName = myNewProfileSplit[0] + "-profile." + myNewProfileSplit[1];
                SocialPhotoHelper.ResizeImageAndUpload(Constants.PHOTO_LOCATION_FROM_VIEW, myNewProfilePhoto.ImageName, myNewProfilePictureImageName, 120);
                thePhotoRepo.AddReferenceToImage(aUser.Model, myNewProfilePhoto.PhotoAlbumId, myNewProfilePictureImageName, true);
            } else {
                throw new CustomException(ErrorKeys.PERMISSION_DENIED);
            }
        }

        public void UploadProfilePicture(AbstractUserModel<T> aUserToUploadFor, HttpPostedFileBase aImageFile) {
            AbstractPhotoAlbumModel<U, V> myProfilePictureAlbum = thePhotoAlbumRepo.GetAbstractProfilePictureAlbumForUser(aUserToUploadFor.Model);
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            V myPhoto = thePhotoRepo.AddReferenceToImage(aUserToUploadFor.Model, myProfilePictureAlbum.Id, myImageName, true);
        }

        public V GetProfilePicture(int aUserId) {
            AbstractPhotoModel<V> myAbstractPhoto = thePhotoRepo.GetAbstractProfilePicture(aUserId);
            if (myAbstractPhoto != null) {
                return myAbstractPhoto.Model;
            }
            return default(V);
        }

        public void UploadImageWithDatabaseReference(AbstractUserModel<T> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile) {
            AbstractPhotoAlbumModel<U, V> myAlbum = thePhotoAlbumRepo.GetAbstractPhotoAlbum(anAlbumId);
            if (myAlbum.CreatedByUserId == aUserToUploadFor.Id) {
                string myImageName = UploadImage(aUserToUploadFor, aImageFile);
                thePhotoRepo.AddReferenceToImage(aUserToUploadFor.Model, anAlbumId, myImageName, false);
            } else {
                new CustomException(UNAUTHORIZED_UPLOAD);
            }
         }

        public void SetPhotoAsAlbumCover(AbstractUserModel<T> myEditingUser, int aPhotoId) {
            AbstractPhotoModel<V> myPhoto = thePhotoRepo.GetAbstractPhoto(aPhotoId);

            if (myPhoto.UploadedByUserId == myEditingUser.Id) {
                thePhotoRepo.SetPhotoAsAlbumCover(aPhotoId);
            } else {
                throw new CustomException(ErrorKeys.PERMISSION_DENIED);
            }
        }

        private string UploadImage(AbstractUserModel<T> aUserToUploadFor, HttpPostedFileBase aImageFile) {
            if(aImageFile == null || !PhotoValidation.IsValidImageFile(aImageFile.FileName)) {
                    throw new CustomException("Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }
            return SocialPhotoHelper.TakeImageAndResizeAndUpload(Constants.PHOTO_LOCATION_FROM_VIEW, aUserToUploadFor.Id.ToString(), aImageFile, MAX_SIZE);
        }

    }
}