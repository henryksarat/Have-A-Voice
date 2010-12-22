using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVFanService {
        void AddFan(User aUser, int aSourceUserId);
        IEnumerable<Fan> FindFansForUser(int aUserId);
        IEnumerable<Fan> FindFansOfUser(int aUserId);
        bool IsFan(int aUserId, User aFan);
    }
}