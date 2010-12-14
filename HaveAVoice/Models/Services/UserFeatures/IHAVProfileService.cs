﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVProfileService {
        ProfileModelBuilder Profile(int aUserId, User aViewingUser);
        IEnumerable<Fan> FindFansForUser(int aUserId);
        IEnumerable<Fan> FindFansOfUser(int aUserId);
    }
}