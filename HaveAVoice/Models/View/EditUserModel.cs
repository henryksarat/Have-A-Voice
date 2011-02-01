using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class EditUserModel {
        public User UserInformation { get; set; }
        public string NewPassword { get; set; }
        public string RetypedPassword { get; set; }
        public string OriginalEmail { get; set; }
        public string OriginalUsername { get; set; }
        public string OriginalPassword { get; set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }

        public EditUserModel(User aUser) {
            UserInformation = aUser;
            NewPassword = string.Empty;
            RetypedPassword = string.Empty;
            OriginalEmail = aUser.Email;
            OriginalUsername = aUser.Username;
            OriginalPassword = aUser.Password;
            ProfilePictureURL = HAVConstants.NO_PROFILE_PICTURE_URL;
            States = new List<SelectListItem>();
        }
    }
}
