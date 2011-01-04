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
        private IHAVFanService theFanService;
        private IHAVUserPictureRepository theUserPictureRepo;
 
        public HAVUserPictureService()
            : this(new HAVFanService(), new EntityHAVUserPictureRepository(), new HAVBaseRepository()) { }

        public HAVUserPictureService(IHAVFanService aFanService, IHAVUserPictureRepository aUserPictureRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theFanService = aFanService;
            theUserPictureRepo = aUserPictureRepo;
        }

        public IEnumerable<UserPicture> GetUserPictures(User aViewingUser, int aUserId) {
            if (aViewingUser.Id == aUserId || theFanService.IsFan(aUserId, aViewingUser)) {
                return theUserPictureRepo.GetUserPictures(aUserId);
            }

            throw new NotFanException();
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return theUserPictureRepo.GetProfilePicture(aUserId);
        }

        public void DeleteUserPictures(List<int> aUserPictureIds) {
            foreach (int userPictureId in aUserPictureIds) {
                theUserPictureRepo.DeleteUserPicture(userPictureId);
            }
        }

        public UserPicture GetUserPicture(int aUserPictureId) {
            return theUserPictureRepo.GetUserPicture(aUserPictureId);
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

        private string UploadImage(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            string[] mySplitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileName = aUserToUploadFor.Id + "_" + DateTime.UtcNow.GetHashCode() + "." + myFileExtension;
            string filePath = HttpContext.Current.Server.MapPath(HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW) + myFileName;
            aImageFile.SaveAs(filePath);
            return myFileName;
        }
    }
}