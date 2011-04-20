using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace UniversityOfMe.Models.View {
    public class EditUserModel : CreateUserModel {
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

        public IEnumerable<SelectListItem> States { get; set; }

        public EditUserModel() { }

        public EditUserModel(User aUser) : base(aUser) {
            NewPassword = string.Empty;
            RetypedPassword = string.Empty;
            ProfilePictureURL = Constants.NO_PROFILE_PICTURE_URL;
            States = new List<SelectListItem>();
            Genders = new List<SelectListItem>();
        }
    }
}