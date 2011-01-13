using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVComplaintService {
        bool IssueComplaint(User aFiledBy, string aComplaint, int aIssueId);
        bool IssueReplyComplaint(User aFiledBy, string aComplaint, int aIssueReplyId);
        bool IssueReplyCommentComplaint(User aFiledBy, string aComplaint, int aIssueReplyCommentId);
        bool ProfileComplaint(User aFiledBy, string aComplaint, int aTowardUserId);
        bool PhotoComplaint(User aFiledBy, string aComplaint, int aPhotoId);
    }
}