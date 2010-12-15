using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVComplaintRepository {
        void AddMergeComplaint(User filedBy, string complaint, int mergeRequestId);
        void AddIssueComplaint(User filedBy, string complaint, int issueId);
        void AddIssueReplyComplaint(User filedBy, string complaint, int issueReplyId);
        void AddIssueReplyCommentComplaint(User filedBy, string complaint, int issueReplyCommentId);
        void AddProfileComplaint(User filedBy, string complaint, int towardUserId);
        void AddUserPictureComplaint(User filedBy, string complaint, int pictureId);
    }
}