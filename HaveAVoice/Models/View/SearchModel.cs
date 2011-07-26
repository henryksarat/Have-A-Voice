using System.Collections.Generic;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class SearchModel<T> {
        public IEnumerable<T> SearchResults { get; set; }
        public IEnumerable<SelectListItem> SearchByOptions { get; set; }
        public IEnumerable<SelectListItem> OrderByOptions { get; set; }
    }
}