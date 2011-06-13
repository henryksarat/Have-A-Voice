using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class CreateClubModel {
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
    }
}