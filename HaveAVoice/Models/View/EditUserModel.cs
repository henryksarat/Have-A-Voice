using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class EditUserModel : CreateUserModel {

        public User UserInformation { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ShortUrl { get; set; }

        public bool HasShortUrl { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OriginalFullName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OriginalGender { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OriginalWebsite { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NewPassword { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RetypedPassword { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OriginalEmail { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OriginalPassword { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ProfilePictureURL { get; set; }

        public EditUserModel() { }

        public EditUserModel(User aUser) {
            UserInformation = aUser;
            NewPassword = string.Empty;
            RetypedPassword = string.Empty;
            ProfilePictureURL = Constants.NO_PROFILE_PICTURE_URL;
            States = new List<SelectListItem>();
            Genders = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                HasShortUrl = true;
            }
        }

        public override User CreateNewModel() {
            User myUser = base.CreateNewModel();
            myUser.Website = Website;
            myUser.AboutMe = AboutMe;
            return myUser;
        }
    }
}
