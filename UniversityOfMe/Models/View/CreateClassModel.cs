using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class CreateClassModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassSubject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassCourse { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassTitle { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }

        public int Year { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Details { get; set; }
    }
}