using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVComplaintService {
        bool MergeComplaint(User aFiledBy, string aComplaint, int aMergeRequestId);
        bool IssueComplaint(User aFiledBy, string aComplaint, int aIssueId);
        bool IssueReplyComplaint(User aFiledBy, string aComplaint, int aIssueReplyId);
        bool IssueReplyCommentComplaint(User aFiledBy, string aComplaint, int aIssueReplyCommentId);
        bool ProfileComplaint(User aFiledBy, string aComplaint, int aTowardUserId);
        bool UserPictureComplaint(User aFiledBy, string aComplaint, int aUserPictureId);
        //void GetComplaintsForUser(User forUser);
    }
}