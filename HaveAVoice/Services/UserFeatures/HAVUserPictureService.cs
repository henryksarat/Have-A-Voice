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
    public class HAVUserPictureService : HAVBaseService, IHAVUserPictureService {
        private IHAVFriendService theFriendService;
        private IHAVUserPictureRepository theUserPictureRepo;
 
        public HAVUserPictureService()
            : this(new HAVFriendService(), new EntityHAVUserPictureRepository(), new HAVBaseRepository()) { }

        public HAVUserPictureService(IHAVFriendService aFriendService, IHAVUserPictureRepository aUserPictureRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theFriendService = aFriendService;
            theUserPictureRepo = aUserPictureRepo;
        }

        public IEnumerable<UserPicture> GetUserPictures(User aViewingUser, int aUserId) {
            if (theFriendService.IsFriend(aUserId, aViewingUser)) {
                return theUserPictureRepo.GetUserPictures(aUserId);
            }

            throw new NotFriendException();
        }

        public void DeleteUserPictures(List<int> aUserPictureIds) {
            foreach (int userPictureId in aUserPictureIds) {
                theUserPictureRepo.DeleteUserPicture(userPictureId);
            }
        }

        public UserPicture GetUserPicture(User aViewingUser, int aUserPictureId) {
            UserPicture myUserPicture = theUserPictureRepo.GetUserPicture(aUserPictureId);
            if (theFriendService.IsFriend(myUserPicture.UserId, aViewingUser)) {
                return myUserPicture;
            }

            throw new NotFriendException();
        }

        public void SetToProfilePicture(User aUser, int aUserPictureId) {
            theUserPictureRepo.SetToProfilePicture(aUser, aUserPictureId);
        }

        public void UploadProfilePicture(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            UserPicture myUserPicture = theUserPictureRepo.AddReferenceToImage(aUserToUploadFor, myImageName);
            theUserPictureRepo.SetToProfilePicture(aUserToUploadFor, myUserPicture.Id);
        }

        public bool IsValidImage(string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile)
                && (anImageFile.EndsWith(".jpg") || anImageFile.EndsWith(".jpeg") || anImageFile.EndsWith(".gif"));
        }

        public void UploadImageWithDatabaseReference(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            UserPicture myUserPicture = theUserPictureRepo.AddReferenceToImage(aUserToUploadFor, myImageName);
        }

        public UserPicture GetProfilePicutre(User aUser) {
            return GetProfilePicture(aUser.Id);
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return theUserPictureRepo.GetProfilePicture(aUserId);
        }

        private string UploadImage(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            if(!IsValidImage(aImageFile.FileName)) {
                    throw new CustomException("Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }
            string[] mySplitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileName = aUserToUploadFor.Id + "_" + DateTime.UtcNow.GetHashCode() + "." + myFileExtension;
            string filePath = HttpContext.Current.Server.MapPath(HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW) + myFileName;
            aImageFile.SaveAs(filePath);
            return myFileName;
        }
    }
}