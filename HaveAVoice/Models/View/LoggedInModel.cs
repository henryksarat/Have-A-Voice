using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public abstract class LoggedInModel {
        public NavigationModel NavigationModel { get; private set; }

        public LoggedInModel(User aUser, SiteSection aSection) {
            NavigationModel = new NavigationModel(aUser, aSection);
        }
    }
}