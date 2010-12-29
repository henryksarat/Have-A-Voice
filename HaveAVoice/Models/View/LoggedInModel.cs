﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class LoggedInModel {
        public User User { get; private set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<FeedModel> FeedModels { get; set; }

        public LoggedInModel(User aUser) {
            User = aUser;
            ProfilePictureURL = HAVConstants.NO_PROFILE_PICTURE_URL;
            FeedModels = new List<FeedModel>();
        }
    }
}