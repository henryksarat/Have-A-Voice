using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.BaseWebsite.Models;
using UniversityOfMe.Services.Notifications;
using Social.Validation;
using UniversityOfMe.Services.Users;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Dating;
using System;

namespace UniversityOfMe.Models.View {
    public class LoggedInWrapperModel<T> : LoggedInModel, ILoggedInModel<T> {
        public User User { get; private set; }

        private T Model;

        public LoggedInWrapperModel(User aUser) : base(aUser)  {
            User = aUser;
        }

        public T Get() {
            return Model;
        }

        public void Set(T aModel) {
            Model = aModel;
        }
    }
}