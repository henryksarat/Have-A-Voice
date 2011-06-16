using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UniversityOfMe.Models.View {
    public class TextbookListModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SearchOption { get; set; }

        public IEnumerable<SelectListItem> SearchOptions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OrderByOption { get; set; }

        public IEnumerable<SelectListItem> OrderByOptions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SearchString { get; set; }

        public IEnumerable<TextBook> Textbooks { get; set; }
    }
}