using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInListModel<T> : LoggedInModel {
        public IEnumerable<T> Models { get; set; }

        public LoggedInListModel(User aUser) : base(aUser, SiteSection.None) {
            Models = new List<T>();
        }

        public LoggedInListModel(User aUser, SiteSection aSection) : base(aUser, aSection) {
            Models = new List<T>();
        }
    }
}