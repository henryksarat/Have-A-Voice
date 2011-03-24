using System;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Issues;
using HaveAVoice.Services.UserFeatures;
using Social.Generic.Models;
using Social.User.Services;

namespace HaveAVoice.Helpers {
    public static class ComplaintHelper {
        public static string IssueReplyLink(int aReplyId) {
            return String.Format("/Complaint/Create?sourceId={0}&complaintType={1}", aReplyId, ComplaintType.IssueReply);
        }

        public static string IssueLink(int anIssueId) {
            return String.Format("<a href=\"/Complaint/Create?sourceId={0}&complaintType={1}\" class=\"issue-report\" alt=\"{2}\">{2}</a>",
                anIssueId,
                ComplaintType.Issue,
                "Report this issue");
        }

        public static string IssueReplyLinkStyled(int aReplyId) {
            return String.Format("<a href=\"/Complaint/Create?sourceId={0}&complaintType={1}\" class=\"issue-report\" alt=\"{2}\">{2}</a>",
                aReplyId,
                ComplaintType.IssueReply,
                "Report this reply to the issue");
        }

        public static string IssueReplyCommentLink(int aComment) {
            return String.Format("<a href=\"/Complaint/Create?sourceId={0}&complaintType={1}\" class=\"issue-report\">{2}</a>",
                aComment,
                ComplaintType.IssueReplyComment,
                "Report Comment");
        }

        public static void FillComplaintModelBuilder(ComplaintModel.Builder aBuilder, IUserRetrievalService<User> aUserRetrievalService, 
                                                     IHAVIssueService aIssueService, IHAVIssueReplyService anIssueReplyService, IHAVIssueReplyCommentService anIssueReplyCommentService, 
                                                     IHAVPhotoService aPhotoService) {
            UserInformationModel<User> myUser = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();
            switch (aBuilder.ComplaintType()) {
                case ComplaintType.Issue:
                    Issue myIssue = aIssueService.GetIssue(aBuilder.SourceId(), myUser);
                    aBuilder.TowardUser(myIssue.User);
                    aBuilder.SourceDescription(myIssue.Title);
                    break;
                case ComplaintType.IssueReply:
                    IssueReply myIssueReply = anIssueReplyService.GetIssueReply(aBuilder.SourceId());
                    aBuilder.TowardUser(myIssueReply.User);
                    aBuilder.SourceDescription(myIssueReply.Reply);
                    break;
                case ComplaintType.IssueReplyComment:
                    IssueReplyComment myIssueReplyComment = anIssueReplyCommentService.GetIssueReplyComment(aBuilder.SourceId());
                    aBuilder.TowardUser(myIssueReplyComment.User);
                    aBuilder.SourceDescription(myIssueReplyComment.Comment);
                    break;
                case ComplaintType.ProfileComplaint:
                    User myTowardUser = aUserRetrievalService.GetUser(aBuilder.SourceId());
                    aBuilder.TowardUser(myTowardUser);
                    aBuilder.SourceDescription("You are reporting this user because of their profile.");
                    break;
                case ComplaintType.PhotoComplaint:
                    Photo myPhoto = aPhotoService.GetPhoto(myUser.Details, aBuilder.SourceId());
                    aBuilder.TowardUser(myPhoto.User);
                    aBuilder.SourceDescription(myPhoto.ImageName);
                    break;
            }
        }
    }
}
