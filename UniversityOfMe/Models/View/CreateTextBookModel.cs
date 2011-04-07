using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class CreateTextBookModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> TextBookConditions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TextBookCondition { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BookTitle { get; set; }

        public HttpPostedFileBase BookImage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassCode { get; set; }

        public IEnumerable<SelectListItem> BuySellOptions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BuySell { get; set; }

        public string Edition { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Price { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Details { get; set; }
    }
}