using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVNotificationRepository {
        IEnumerable<Board> UnreadBoardMessages(User aUser);
        IEnumerable<BoardViewedState> UnreadParticipatingBoardMessages(User aUser);
        IEnumerable<IssueViewedState> UnreadIssues(User aUser);
    }
}