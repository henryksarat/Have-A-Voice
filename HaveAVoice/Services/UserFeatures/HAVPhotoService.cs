using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPhotoService : HAVBaseService, IHAVPhotoService {
        private IHAVFriendService theFriendService;
        private IHAVPhotoRepository thePhotoRepo;
 
        public HAVPhotoService()
            : this(new HAVFriendService(), new EntityHAVPhotoRepository(), new HAVBaseRepository()) { }

        public HAVPhotoService(IHAVFriendService aFriendService, IHAVPhotoRepository aPhotoRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theFriendService = aFriendService;
            thePhotoRepo = aPhotoRepo;
        }

        public void CreatePhotoAlbum(User aUser, string aName, string aDescription) {
            thePhotoRepo.CreatePhotoAlbum(aUser, aName, aDescription);
        }

        public IEnumerable<Photo> GetPhotos(User aViewingUser, int anAlbumId, int aUserId) {
            if (theFriendService.IsFriend(aUserId, aViewingUser)) {
                return thePhotoRepo.GetPhotos(aUserId, anAlbumId);
            }

            throw new NotFriendException();
        }

        public void DeletePhotos(List<int> aPhotoIds) {
            foreach (int aPhotoId in aPhotoIds) {
                thePhotoRepo.DeletePhoto(aPhotoId);
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
            PhotoAlbum myProfilePictureAlbum = thePhotoRepo.GetProfilePictureAlbumForUser(aUserToUploadFor);
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            Photo myPhoto = thePhotoRepo.AddReferenceToImage(aUserToUploadFor, myProfilePictureAlbum.Id, myImageName);
            thePhotoRepo.SetToProfilePicture(aUserToUploadFor, myPhoto.Id);
        }

        public bool IsValidImage(string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile)
                && (anImageFile.ToUpper().EndsWith(".JPG") || anImageFile.ToUpper().EndsWith(".JPEG") || anImageFile.ToUpper().EndsWith(".GIF"));
        }

        private void UploadImageWithDatabaseReference(User aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile) {
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            thePhotoRepo.AddReferenceToImage(aUserToUploadFor, anAlbumId, myImageName);
        }

        public Photo GetProfilePicutre(User aUser) {
            return GetProfilePicture(aUser.Id);
        }

        public Photo GetProfilePicture(int aUserId) {
            return thePhotoRepo.GetProfilePicture(aUserId);
        }

        private string UploadImage(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            if(!IsValidImage(aImageFile.FileName)) {
                    throw new CustomException("Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }
            string[] mySplitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileName = aUserToUploadFor.Id + "_" + DateTime.UtcNow.GetHashCode() + "." + myFileExtension;
            string filePath = HttpContext.Current.Server.MapPath(HAVConstants.PHOTO_LOCATION_FROM_VIEW) + myFileName;
            aImageFile.SaveAs(filePath);
            return myFileName;
        }
    }
}