using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class EditUserModel {
        public User UserInformation { get; set; }
        public string OriginalFullName { get; set; }
        public string OriginalGender { get; set; }
        public string OriginalWebsite { get; set; }
        public string NewPassword { get; set; }
        public string RetypedPassword { get; set; }
        public string OriginalEmail { get; set; }
        public string OriginalPassword { get; set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }

        public EditUserModel(User aUser) {
            UserInformation = aUser;
            NewPassword = string.Empty;
            RetypedPassword = string.Empty;
            ProfilePictureURL = Constants.NO_PROFILE_PICTURE_URL;
            States = new List<SelectListItem>();
            Genders = new List<SelectListItem>();
        }
    }
}
