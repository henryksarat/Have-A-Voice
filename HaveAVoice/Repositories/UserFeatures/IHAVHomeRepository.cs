using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVHomeRepository {
        IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition);
        IEnumerable<IssueReply> NewestIssueReplys();
        IEnumerable<IssueReply> GetMostPopularIssueReplys();
    }
}