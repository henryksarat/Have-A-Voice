﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVProfileService {
        UserProfileModel Profile(int aUserId, User aViewingUser);
        UserProfileModel Profile(string aShortUrl, User aViewingUser);
        UserProfileModel MyProfile(User aUser);
        UserProfileModel UserIssueActivity(int aUserId, User aViewingUser);
        UserProfileModel AuthorityProfile(UserInformationModel anAuthorityUser);
    }
}