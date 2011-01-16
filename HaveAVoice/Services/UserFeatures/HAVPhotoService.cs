using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;
using System.IO;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPhotoService : HAVBaseService, IHAVPhotoService {
        private const string UNAUTHORIZED_UPLOAD = "You are not allowed to upload to that album.";

        private IHAVFriendService theFriendService;
        private IHAVPhotoAlbumRepository thePhotoAlbumRepo;
        private IHAVPhotoRepository thePhotoRepo;
 
        public HAVPhotoService()
            : this(new HAVFriendService(), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository(), new HAVBaseRepository()) { }

        public HAVPhotoService(IHAVFriendService aFriendService, IHAVPhotoAlbumRepository aPhotoAlbumRepo, IHAVPhotoRepository aPhotoRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            thePhotoAlbumRepo = aPhotoAlbumRepo;
            theFriendService = aFriendService;
            thePhotoRepo = aPhotoRepo;
        }

        public IEnumerable<Photo> GetPhotos(User aViewingUser, int anAlbumId, int aUserId) {
            if (theFriendService.IsFriend(aUserId, aViewingUser)) {
                return thePhotoRepo.GetPhotos(aUserId, anAlbumId);
            }

            throw new NotFriendException();
        }

        public void DeletePhoto(User aUserDeleting, int aPhotoId) {
            Photo myPhoto = GetPhoto(aUserDeleting, aPhotoId);
            if (myPhoto.UploadedByUserId == aUserDeleting.Id) {
                FileInfo myFile = new FileInfo(HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(myPhoto.ImageName)));
                if (myFile.Exists) {
                    myFile.Delete();
                } else {
                    throw new FileNotFoundException();
                }

                thePhotoRepo.DeletePhoto(aPhotoId);
            } else {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }
        }

        public Photo GetPhoto(User aViewingUser,  int aPhotoId) {
            Photo myPhotoId = thePhotoRepo.GetPhoto(aPhotoId);
            if (theFriendService.IsFriend(myPhotoId.UploadedByUserId, aViewingUser)) {
                return myPhotoId;
            }

            throw new NotFriendException();
        }

        public void SetToProfilePicture(User aUser, int aPhotoId) {
            thePhotoRepo.SetToProfilePicture(aUser, aPhotoId);
        }

        public void UploadProfilePicture(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            PhotoAlbum myProfilePictureAlbum = thePhotoAlbumRepo.GetProfilePictureAlbumForUser(aUserToUploadFor);
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            Photo myPhoto = thePhotoRepo.AddReferenceToImage(aUserToUploadFor, myProfilePictureAlbum.Id, myImageName);
            thePhotoRepo.SetToProfilePicture(aUserToUploadFor, myPhoto.Id);
        }

        public bool IsValidImage(string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile)
                && (anImageFile.ToUpper().EndsWith(".JPG") || anImageFile.ToUpper().EndsWith(".JPEG") || anImageFile.ToUpper().EndsWith(".GIF"));
        }

        public Photo GetProfilePicutre(User aUser) {
            return GetProfilePicture(aUser.Id);
        }

        public Photo GetProfilePicture(int aUserId) {
            return thePhotoRepo.GetProfilePicture(aUserId);
        }

        public void UploadImageWithDatabaseReference(UserInformationModel aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile) {
            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);
             if (myAlbum.CreatedByUserId == aUserToUploadFor.Details.Id) {
                 string myImageName = UploadImage(aUserToUploadFor.Details, aImageFile);
                 thePhotoRepo.AddReferenceToImage(aUserToUploadFor.Details, anAlbumId, myImageName);
             }

             new CustomException(UNAUTHORIZED_UPLOAD);
         }

        private string UploadImage(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            if(!IsValidImage(aImageFile.FileName)) {
                    throw new CustomException("Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }
            string[] mySplitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileName = aUserToUploadFor.Id + "_" + DateTime.UtcNow.GetHashCode() + "." + myFileExtension;
            string filePath = HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(myFileName));
            aImageFile.SaveAs(filePath);
            return myFileName;
        }
    }
}