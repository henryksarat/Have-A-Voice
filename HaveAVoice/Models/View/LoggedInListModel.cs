using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using Social.BaseWebsite.Models;

namespace HaveAVoice.Models.View {
    public class LoggedInListModel<T> : LoggedInModel, ILoggedInListModel<T> {
        public IEnumerable<T> Models { get; set; }

        public LoggedInListModel(User aUsersPanelToDisplay, SiteSection aSection) 
            : this(aUsersPanelToDisplay, aUsersPanelToDisplay, aSection) { }

        public LoggedInListModel(User aUsersPanelToDisplay, User aLoggedInUser, SiteSection aSection)
            : base(aUsersPanelToDisplay, aLoggedInUser, aSection) {
            Models = new List<T>();
        }

        public IEnumerable<T> Get() {
            return Models;
        }

        public void Set(IEnumerable<T> aModel) {
            Models = aModel;
        }
    }
}