using UniversityOfMe.Models.Social;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace UniversityOfMe.Models.View {
    public class CreateUserModel : SocialUserModel {
        public IEnumerable<SelectListItem> Genders { get; set; }

        public CreateUserModel() {
            DateOfBirth = DateTime.UtcNow;
        }

        public string getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}