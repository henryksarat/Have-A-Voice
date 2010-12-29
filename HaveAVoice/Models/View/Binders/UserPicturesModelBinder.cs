using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View {
    public class UserPicturesModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVUserPictureService myUserPictureService = new HAVUserPictureService();
            User myUserInfo = HAVUserInformationFactory.GetUserInformation().Details;

            int userId = Int32.Parse(BinderHelper.GetA(bindingContext, "UserId"));
            string myProfilePictureURL = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string selectedProfilePictureIds = BinderHelper.GetA(bindingContext, "SelectedProfilePictureId").Trim();

            IEnumerable<UserPicture> userPictures = myUserPictureService.GetUserPictures(myUserInfo, userId);
            UserPicture profilePicture = myUserPictureService.GetProfilePicture(userId);

            string[] splitIds = selectedProfilePictureIds.Split(',');
            List<int> selectedProfilePictures = new List<int>();

            foreach (string id in splitIds) {
                if (id != string.Empty) {
                    selectedProfilePictures.Add(Int32.Parse(id));
                }
            }

            return new UserPicturesModel() {
                UserId = userId,
                ProfilePictureURL = myProfilePictureURL,
                UserPictures = userPictures,
                SelectedUserPictures = selectedProfilePictures
            };
        }
    }
}
