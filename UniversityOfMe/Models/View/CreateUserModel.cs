using UniversityOfMe.Models.SocialModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using UniversityOfMe.Models.View.Search;

namespace UniversityOfMe.Models.View {
    public class CreateUserModel : SocialUserModel {
        public IEnumerable<SelectListItem> Genders { get; set; }
        public int RegisteredUserCount { get; set; }
        public MarketplaceItem NewestItem { get; set; }
        public TextBook NewestTextbook { get; set; }
        public SearchResultsModel LatestResults { get; set; }

        public CreateUserModel() {
            DateOfBirth = DateTime.UtcNow;
        }

        protected CreateUserModel(User aUser)
            : base(aUser) { }

        public string getDateOfBirthFormatted() {
            if (DateOfBirth.HasValue) {
                return DateOfBirth.Value.ToString("MM-dd-yyyy");
            } else {
                return string.Empty;
            }
        }
    }
}