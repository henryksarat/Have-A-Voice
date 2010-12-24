using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVFanRepository {
        void AddFan(User aUser, int aSourceUserId);
        void ApproveFan(int aFanId);
        void DeleteFan(int aFanId);
        IEnumerable<Fan> FindFansForUser(int aUserId);
        IEnumerable<Fan> FindFansOfUser(int aUserId);
        IEnumerable<Fan> FindPendingFansForUser(int aUserId);
        bool IsFan(int aUserId, User aFan);
        bool IsPending(int aUserId, User aFan);
    }
}