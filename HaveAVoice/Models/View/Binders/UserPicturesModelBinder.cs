using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View {
    public class PhotosModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVPhotoService myPhotoService = new HAVPhotoService();
            User myUserInfo = HAVUserInformationFactory.GetUserInformation().Details;

            int myUserId = Int32.Parse(BinderHelper.GetA(bindingContext, "UserId"));
            string myProfilePictureURL = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string selectedProfilePictureIds = BinderHelper.GetA(bindingContext, "SelectedProfilePictureId").Trim();

            IEnumerable<Photo> myPhotos = myPhotoService.GetPhotos(myUserInfo, myUserId);
            Photo myProfilePicture = myPhotoService.GetProfilePicture(myUserId);

            string[] splitIds = selectedProfilePictureIds.Split(',');
            List<int> selectedProfilePictures = new List<int>();

            foreach (string id in splitIds) {
                if (id != string.Empty) {
                    selectedProfilePictures.Add(Int32.Parse(id));
                }
            }

            return new PhotosModel() {
                UserId = myUserId,
                ProfilePictureURL = myProfilePictureURL,
                Photos = myPhotos,
                SelectedPhotos = selectedProfilePictures
            };
        }
    }
}
