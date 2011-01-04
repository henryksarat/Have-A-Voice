using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInModelWithItemWrapped<T> {
        public UserModel UserModel { get; private set; }
        public T Model { get; set; }

        public LoggedInModelWithItemWrapped(User aUser) {
            UserModel = new UserModel(aUser);
        }
    }
}