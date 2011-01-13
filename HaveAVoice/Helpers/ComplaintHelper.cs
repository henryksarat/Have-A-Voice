﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Helpers
{
    public static class ComplaintHelper
    {
        public static string IssueReplyLink(int aIssueReply) {
            return String.Format("<a href=\"/Complaint/Complaint?sourceId={0}&complaintType={1}\">{2}</a>",
                aIssueReply,
                ComplaintType.IssueReply,
                "Report");
        }

        public static string IssueLink(int aIssue) {
            return String.Format("<a href=\"/Complaint/Complaint?sourceId={0}&complaintType={1}\">{2}</a>",
                aIssue,
                ComplaintType.Issue,
                "Report Issue");
        }

        public static string IssueReplyCommentLink(int aComment) {
            return String.Format("<a href=\"/Complaint/Complaint?sourceId={0}&complaintType={1}\">{2}</a>",
                aComment,
                ComplaintType.IssueReplyComment,
                "Report Comment");
        }

        public static void FillComplaintModelBuilder(ComplaintModel.Builder aBuilder, IHAVUserRetrievalService aUserRetrievalService, IHAVIssueService aIssueService, IHAVPhotoService aPhotoService) {
            User myUser = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation().Details;
            switch (aBuilder.ComplaintType()) {
                case ComplaintType.Issue:
                    Issue myIssue = aIssueService.GetIssue(aBuilder.SourceId());
                    aBuilder.TowardUser(myIssue.User);
                    aBuilder.SourceDescription(myIssue.Title);
                    break;
                case ComplaintType.IssueReply:
                    IssueReply myIssueReply = aIssueService.GetIssueReply(aBuilder.SourceId());
                    aBuilder.TowardUser(myIssueReply.User);
                    aBuilder.SourceDescription(myIssueReply.Reply);
                    break;
                case ComplaintType.IssueReplyComment:
                    IssueReplyComment myIssueReplyComment = aIssueService.GetIssueReplyComment(aBuilder.SourceId());
                    aBuilder.TowardUser(myIssueReplyComment.User);
                    aBuilder.SourceDescription(myIssueReplyComment.Comment);
                    break;
                case ComplaintType.ProfileComplaint:
                    User myTowardUser = aUserRetrievalService.GetUser(aBuilder.SourceId());
                    aBuilder.TowardUser(myTowardUser);
                    aBuilder.SourceDescription("You are reporting this user because of their profile.");
                    break;
                case ComplaintType.PhotoComplaint:
                    Photo myPhoto = aPhotoService.GetPhoto(myUser, aBuilder.SourceId());
                    aBuilder.TowardUser(myPhoto.User);
                    aBuilder.SourceDescription(myPhoto.ImageName);
                    break;
            }
        }
    }
}
