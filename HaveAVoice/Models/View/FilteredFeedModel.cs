using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class FilteredFeedModel {
        public NavigationModel NavigationModel { get; private set; }
        public IEnumerable<FeedModel> FeedModels { get; set; }
        public IEnumerable<SelectListItem> States { get; private set; }

        public FilteredFeedModel(User aUser) {
            NavigationModel = new NavigationModel(aUser, SiteSection.Home);
            FeedModels = new List<FeedModel>();
            States = new SelectList(HAVConstants.STATES, "Select");
        }
    }
}