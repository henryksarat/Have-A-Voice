using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVFanRepository {
        void Add(User aUser, int aSourceUserId);
        void Remove(User aUser, int aSourceUserId);
        IEnumerable<Fan> GetAllFansForUser(User aUser);
        IEnumerable<Fan> GetAllSourceUsersForFan(User aUser);
        bool IsFan(User aFanUser, int aSourceUserId);
    }
}