using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Repositories.UserFeatures;
using Social.Generic.Constants;
using Social.User.Services;

namespace HaveAVoice.Models.View {
    public class EditUserModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IUserRetrievalService<User> myUserRetrievalService = new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository());

            string myProfilePictureUrl = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string myOriginalEmail = BinderHelper.GetA(bindingContext, "OriginalEmail");
            string myOriginalPassword = BinderHelper.GetA(bindingContext, "OriginalPassword");
            string myOriginalGender = BinderHelper.GetA(bindingContext, "OriginalGender");
            string myOriginalWebsite = BinderHelper.GetA(bindingContext, "OriginalWebSite");
            string myOriginalFullName = BinderHelper.GetA(bindingContext, "OriginalFullName");
            string myNewPassword = BinderHelper.GetA(bindingContext, "NewPassword");
            string myRetypedPassword = BinderHelper.GetA(bindingContext, "RetypedPassword");
            string myWebsite = BinderHelper.GetA(bindingContext, "Website");
            string myCity = BinderHelper.GetA(bindingContext, "City");
            string myState = BinderHelper.GetA(bindingContext, "State");
            string myGender = BinderHelper.GetA(bindingContext, "Gender");
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
            user.DateOfBirth = Convert.ToDateTime(BinderHelper.GetA(bindingContext, "DateOfBirth"));
            user.AboutMe = myAboutMe;
            user.Gender = myGender;

            IEnumerable<SelectListItem> myStates = new SelectList(UnitedStates.STATES, user.State);
            IEnumerable<SelectListItem> myGenders = new SelectList(Constants.GENDERS, user.Gender);

            return new EditUserModel(user) {
                States = myStates,
                Genders = myGenders,
                ProfilePictureURL = myProfilePictureUrl,
                NewPassword = myNewPassword,
                RetypedPassword = myRetypedPassword,
                OriginalEmail = myOriginalEmail,
                OriginalPassword = myOriginalPassword,
                OriginalFullName = myOriginalFullName,
                OriginalWebsite = myOriginalWebsite,
                OriginalGender = myOriginalGender
            };
        }
    }
}
