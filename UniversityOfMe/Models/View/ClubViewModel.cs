using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class ClubViewModel {
        public int ClubId { get; set; }
        public IEnumerable<SelectListItem> ClubTypes { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClubType { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
        public HttpPostedFileBase ClubImage { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClubImageUrl { get; set; }

        public ClubViewModel() { }

        public ClubViewModel(Club aClub) {
            ClubId = aClub.Id;
            Name = aClub.Name;
            Description = aClub.Description;
            ClubImageUrl = PhotoHelper.ClubPhoto(aClub);
        }
    }
}