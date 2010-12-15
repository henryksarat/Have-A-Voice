using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVProfileRepository {

        IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser);
        bool IsFan(int aUserId, User aFan);
        IEnumerable<Fan> FindFansForUser(int aUserId);
        IEnumerable<Fan> FindFansOfUser(int aUserId);
    }
}