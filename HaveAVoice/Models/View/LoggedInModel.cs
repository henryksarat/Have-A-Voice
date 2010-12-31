using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInModel<T> {
        public UserModel UserModel { get; private set; }
        public IEnumerable<T> Models { get; set; }

        public LoggedInModel(UserModel aUser) {
            UserModel = aUser;
            Models = new List<T>();
        }
    }
}