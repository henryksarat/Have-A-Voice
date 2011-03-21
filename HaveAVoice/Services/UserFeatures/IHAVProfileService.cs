﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVProfileService {
        UserProfileModel Profile(int aUserId, User aViewingUser);
        UserProfileModel Profile(string aShortUrl, User aViewingUser);
        UserProfileModel MyProfile(User aUser);
        UserProfileModel UserIssueActivity(int aUserId, User aViewingUser);
        UserProfileModel AuthorityProfile(UserInformationModel<User> anAuthorityUser);
    }
}