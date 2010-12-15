using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;
using System.Collections.Generic;

namespace HaveAVoice.Models.View {
    public class EditUserModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVUserService myUserService = new HAVUserService(new ModelStateWrapper(null));

            string myProfilePictureUrl = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string myOriginalEmail = BinderHelper.GetA(bindingContext, "OriginalEmail");
            string myOriginalUsername = BinderHelper.GetA(bindingContext, "OriginalUsername");
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

            User user = CloneHelper.CloneUserWithReflection(myUserService.GetUser(userId));
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

            IEnumerable<SelectListItem> myTimezones = new SelectList(TimezoneHelper.GetTimeZones(), TimezoneHelper.GetTimezone(user.UTCOffset));
            IEnumerable<SelectListItem> myStates = new SelectList(HAVConstants.STATES, user.State);

            return new EditUserModel.Builder(user)
            .setTimezones(myTimezones)
            .setStates(myStates)
            .setImageFile(myImageFile)
            .setProfilePictureUrl(myProfilePictureUrl)
            .setNewPassword(myNewPassword)
            .setRetypedPassword(myRetypedPassword)
            .setOriginalEmail(myOriginalEmail)
            .setOriginalUsername(myOriginalUsername)
            .Build();
        }
    }
}
