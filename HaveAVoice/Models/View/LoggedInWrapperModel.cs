using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using Social.BaseWebsite.Models;

namespace HaveAVoice.Models.View {
    public class LoggedInWrapperModel<T> : LoggedInModel, ILoggedInModel<T> {
        public T Model { get; set; }

        public LoggedInWrapperModel(User aUserOfPanel, SiteSection aSection) 
            : this(aUserOfPanel, aUserOfPanel, aSection) { }

        public LoggedInWrapperModel(User aUserOfPanel, User aLoggedInUser, SiteSection aSection) 
            : base(aUserOfPanel, aLoggedInUser, aSection) { }

        public T Get() {
            return Model;
        }

        public void Set(T aModel) {
            Model = aModel;
        }
    }
}