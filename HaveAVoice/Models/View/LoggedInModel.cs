using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public abstract class LoggedInModel {
        public UserModel UserModel { get; private set; }

        public LoggedInModel(User aUser) {
            UserModel = new UserModel(aUser);
        }
    }
}