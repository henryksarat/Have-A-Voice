using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVFanService {
        void AddFan(User aUser, int aSourceUserId);
        void ApproveFan(int aFanId);
        void DeclineFan(int aFanId);
        IEnumerable<Fan> FindFansForUser(int aUserId);
        IEnumerable<Fan> FindPendingFansForUser(int aUserId);
        bool IsFan(int aSourceUserId, User aFanUser);
        bool IsPending(int aUserId, User aFan);
    }
}