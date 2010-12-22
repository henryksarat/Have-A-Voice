using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVProfileService {
        ProfileModelBuilder Profile(int aUserId, User aViewingUser);
    }
}