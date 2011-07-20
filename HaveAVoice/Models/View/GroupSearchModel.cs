﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class GroupSearchModel {
        public IEnumerable<Group> SearchResults { get; set; }
        public IEnumerable<SelectListItem> SearchByOptions { get; set; }
        public IEnumerable<SelectListItem> OrderByOptions { get; set; }
        public bool MyGroups { get; set; }
    }
}