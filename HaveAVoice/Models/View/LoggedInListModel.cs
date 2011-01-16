using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInListModel<T> : LoggedInModel {
        public IEnumerable<T> Models { get; set; }
        public int SourceUserIdOfContent { get; private set; }

        public LoggedInListModel(User aLoggedInUser, SiteSection aSection) : base(aLoggedInUser, aSection) {
            Models = new List<T>();
            SourceUserIdOfContent = aLoggedInUser.Id;
        }

        public LoggedInListModel(User aLoggedInUser, SiteSection aSection, int aSourceUserIdOfContent) : base(aLoggedInUser, aSection) {
            Models = new List<T>();
            SourceUserIdOfContent = aSourceUserIdOfContent;
        }
    }
}