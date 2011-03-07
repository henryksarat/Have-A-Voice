using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Models.View;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyHelper {
        public static string IssueReplyDisplay(IEnumerable<IssueReply> anIssueReplies) {
            string myList = string.Empty;

            int myCount = 0;

            foreach (IssueReply myIssueReply in anIssueReplies) {
                if (myCount >= 4) {
                    break;
                }
                var myLI = new TagBuilder("li");
                myLI.InnerHtml = myIssueReply.Reply;
                myList += myLI.ToString();

                myCount++;
            }


            return myList;
        }

        public static bool ShouldDisplayEditLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Any_Issue_Reply);
        }

        public static bool ShouldDisplayDeleteLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Any_Issue_Reply);
        }
    }
}