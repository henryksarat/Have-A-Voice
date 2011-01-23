using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;
using System.Collections.Generic;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Models.View {
    public class EditUserModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVUserRetrievalService myUserRetrievalService = new HAVUserRetrievalService();

            string myProfilePictureUrl = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string myOriginalEmail = BinderHelper.GetA(bindingContext, "OriginalEmail");
            string myOriginalUsername = BinderHelper.GetA(bindingContext, "OriginalUsername");
            string myOriginalPassword = BinderHelper.GetA(bindingContext, "OriginalPassword");
            string myNewPassword = BinderHelper.GetA(bindingContext, "NewPassword");
            string myRetypedPassword = BinderHelper.GetA(bindingContext, "RetypedPassword");
            string myWebsite = BinderHelper.GetA(bindingContext, "Website");
            string myTimezone = BinderHelper.GetA(bindingContext, "Timezone");
            string myCity = BinderHelper.GetA(bindingContext, "City");
            string myState = BinderHelper.GetA(bindingContext, "State");
            string myAboutMe = BinderHelper.GetA(bindingContext, "AboutMe");
            HttpPostedFileBase myImageFile = null;
            foreach(string inputTagName in controllerContext.RequestContext.HttpContext.Request.Files) {
                 myImageFile = controllerContext.RequestContext.HttpContext.Request.Files[inputTagName];
            }          

            int userId = Int32.Parse(BinderHelper.GetA(bindingContext, "UserId"));

            User user = CloneHelper.CloneUserWithReflection(myUserRetrievalService.GetUser(userId));
            user.Email = BinderHelper.GetA(bindingContext, "Email");
            user.FirstName = BinderHelper.GetA(bindingContext, "FirstName");
            user.LastName = BinderHelper.GetA(bindingContext, "LastName");
            user.Password = BinderHelper.GetA(bindingContext, "Password");
            user.City = myCity;
            user.State = myState;
            user.Website = myWebsite;
            user.Newsletter = Boolean.Parse(BinderHelper.GetA(bindingContext, "Newsletter"));
            user.DateOfBirth = Convert.ToDateTime(BinderHelper.GetA(bindingContext, "DateOfBirth"));
            user.UTCOffset = TimezoneHelper.GetOffset(myTimezone);
            user.AboutMe = myAboutMe;

            IEnumerable<SelectListItem> myStates = new SelectList(HAVConstants.STATES, user.State);

            return new EditUserModel(user) {
                States = myStates,
                ImageFile = myImageFile,
                ProfilePictureURL = myProfilePictureUrl,
                NewPassword = myNewPassword,
                RetypedPassword = myRetypedPassword,
                OriginalEmail = myOriginalEmail,
                OriginalUsername = myOriginalUsername,
                OriginalPassword = myOriginalPassword
            };
        }
    }
}
