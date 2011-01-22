using System;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Helpers;
using System.Collections.Generic;

namespace HaveAVoice.Helpers.UI {
    public class IssueHelper {
        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            return BuildIssueReplyTable(anIssueReply);
        }

        public static string OfficialIssueReply(IssueReplyModel anIssueReply, IssueFilter aIssueFilter) {
            return BuildIssueReplyTable(anIssueReply);
        }

        private static string BuildIssueReplyTable(IssueReplyModel anIssueReply) {
        	var stanceDiv = new TagBuilder("div");

            if(anIssueReply.IssueStance == (int)IssueStance.Agree) {
        	    stanceDiv.MergeAttribute("class", "agree m-btm10");
            } else {
                stanceDiv.MergeAttribute("class", "disagree m-btm10");
            }

			var profileDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStance.Disagree) {
			    profileDiv.MergeAttribute("class", "col-3 center push-21");
			} else {
				profileDiv.MergeAttribute("class", "push-6 col-3 center");
			}

			var profileImg = new TagBuilder("img");
			if (anIssueReply.Anonymous) {
				profileImg.MergeAttribute("alt", "Anonymous");
                profileImg.MergeAttribute("src", "http://images.chron.com/photos/2008/05/19/graphic_defaultAvatar/graphic_defaultAvatar.jpg");
			}  else {
				profileImg.MergeAttribute("alt", anIssueReply.User.Username);
				profileImg.MergeAttribute("src", PhotoHelper.ProfilePicture(anIssueReply.User));
			}
			profileImg.MergeAttribute("class", "profile");
			
			profileDiv.InnerHtml += profileImg.ToString();
			stanceDiv.InnerHtml += profileDiv.ToString();
			
			var stanceComment = new TagBuilder("div");
			stanceComment.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
			
			var spanDirSpeak = new TagBuilder("span");
			
            if (anIssueReply.IssueStance == (int)IssueStance.Agree) {
                spanDirSpeak.MergeAttribute("class", "speak-lft");
            } else {
                spanDirSpeak.MergeAttribute("class", "speak-rgt");
            }
			spanDirSpeak.InnerHtml = "&nbsp;";
			
			stanceComment.InnerHtml += spanDirSpeak.ToString();

			var divCommentPad = new TagBuilder("div");
			divCommentPad.MergeAttribute("class","p-a10");
			
			var hrefName = new TagBuilder("a");
			hrefName.MergeAttribute("class", "name");
			if (anIssueReply.Anonymous)
			{
				hrefName.InnerHtml = "Anonymous";
				hrefName.MergeAttribute("href", "#");
			} else {
				hrefName.InnerHtml = anIssueReply.User.Username;
                hrefName.MergeAttribute("href", LinkHelper.ProfilePage(anIssueReply.User));
			}
			
			divCommentPad.InnerHtml += hrefName.ToString();
			divCommentPad.InnerHtml += anIssueReply.Reply;

			var optionWrpr = new TagBuilder("div");
			optionWrpr.MergeAttribute("col-11")

            var optionsDiv = new TagBuilder("div");
            optionsDiv.MergeAttribute("p-v10")

			var editDiv = new TagBuilder("div");
			editDiv.MergeAttribute("col-2");
			
            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();
            if(anIssueReply.User.Id == myUserInformationModel.Details.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue_Reply)) {
                var myEdit = new TagBuilder("a");
                myEdit.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReply.Id));
                myEdit.InnerHtml += "Edit";
                editDiv.InnerHtml +=myEdit.ToString();
            }
            optionsDiv.InnerHtml += editDiv.ToString();
            
            var deleteDiv = new TagBuilder("div");
            deleteDiv.MergeAttribute("col-3");
            
            if(anIssueReply.User.Id == myUserInformationModel.Details.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue_Reply)) {
                var myDelete = new TagBuilder("a");
                myDelete.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReply.Id));
                myDelete.InnerHtml += "Delete";
                deleteDiv.InnerHtml +=myDelete.ToString();
            }
            optionsDiv.InnerHtml += deleteDiv.ToString();
            
            var likeDiv = new TagBuilder("div");
            likeDiv.MergeAttribute("col-3");
            
            var dislikeDiv = new TagBuilder("div");
            dislikeDiv.MergeAttribute("col-3");
            
            if(!anIssueReply.HasDisposition) {
                var myLikeDisposition = new TagBuilder("a");
                myLikeDisposition.MergeAttribute("href", LinkHelper.LikeIssueReply(anIssueReply.Id, anIssueReply.Issue.Id));
                myLikeDisposition.InnerHtml += "Like";
                likeDiv.InnerHtml +=myLikeDisposition.ToString();

                var myDislikeDisposition = new TagBuilder("a");
                myDislikeDisposition.MergeAttribute("href", LinkHelper.DislikeIssueReply(anIssueReply.Id, anIssueReply.Issue.Id));
                myDislikeDisposition.InnerHtml += "Dislike";
                dislikeDiv.InnerHtml +=myDislikeDisposition.ToString();
            }

			optionsDiv.InnerHtml += likeDiv.ToString();
			optionsDiv.InnerHtml += dislikeDiv.ToString();
			
			optionWrpr.InnerHtml += optionsDiv.ToString();
			
			divCommentPad.InnerHtml += optionWrpr.ToString();
			
			stanceComment.InnerHtml += divCommentPad.ToString();

			var divTimeStamp = new TagBuilder("div");
			
            if (anIssueReply.IssueStance == (int)IssueStance.Disagree) {
			    divTimeStamp.MergeAttribute("class", "col-3 date-tile pull-9");
            } else {
            	divTimeStamp.MergeAttribute("class", "col-3 date-tile push-6");
            }
            
            stanceDiv.InnerHtml += stanceComment.ToString();
			
			var divTimePad = new TagBuilder("div");
			divTimePad.MergeAttribute("class", "p-a10");

			var spanTime = new TagBuilder("span");
			spanTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();
			
			divTimePad.InnerHtml += spanTime.ToString();
			divTimePad.InnerHtml += "&nbsp;";
			divTimePad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");
			
			divTimeStamp.InnerHtml += divTimePad.ToString();
			
			stanceDiv.InnerHtml += divTimeStamp.ToString();
			
			var clrDiv = new TagBuilder("div");
			clrDiv.MergeAttribute("class", "clear");
			clrDiv.InnerHtml += "&nbsp;";

			stanceDiv.InnerHtml += clrDiv.ToString();

        	return stanceDiv.ToString(TagRenderMode.Normal);
        }

        public static string Comment(IssueReplyComment aComment) {
        	var wrprDiv = new TagBuilder("div");
        	wrprDiv.MergeAttribute("class", "m-btm10");
        	
        	var profileDiv = new TagBuilder("div");
        	profileDiv.MergeAttribute("class", "push-6 col-3 center");

            var profileImg = new TagBuilder("img");
        	profileImg.MergeAttribute("class", "profile");
        	profileImg.MergeAttribute("alt", aComment.User.Username);
        	profileImg.MergeAttribute("src", "/Photos/no_profile_picture.jpg");
        	
        	profileDiv.InnerHtml += profileImg.ToString();
        	wrprDiv.InnerHtml += profileDiv.ToString();
        	
        	var commentDiv = new TagBuilder("div");
        	commentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt row");
        	
			var spanDirSpeak = new TagBuilder("span");
            spanDirSpeak.MergeAttribute("class", "speak-lft");
			spanDirSpeak.InnerHtml = "&nbsp;";
			
			commentDiv.InnerHtml += spanDirSpeak.ToString();
        	
        	var paddingDiv = new TagBuilder("div");
        	paddingDiv.MergeAttribute("class", "p-a10");
        	
        	var href = new TagBuilder("a");
        	href.MergeAttribute("class", "name");
        	href.MergeAttribute("href", "#");
        	href.InnerHtml += aComment.User.Username;
        	
        	paddingDiv.InnerHtml += href.ToString();
        	paddingDiv.InnerHtml += "&nbsp;";
        	paddingDiv.InnerHtml += aComment.Comment;
        	
        	paddingDiv.InnerHtml += ComplaintHelper.IssueReplyCommentLink(aComment.Id);
        	
        	commentDiv.InnerHtml += paddingDiv.ToString();
        	wrprDiv.InnerHtml += commentDiv.ToString();
        	
        	var divTime = new TagBuilder("div");
            divTime.MergeAttribute("class", "push-6 col-3 date-tile");
        	
			var divTimePad = new TagBuilder("div");
			divTimePad.MergeAttribute("class", "p-a10");

			var spanTime = new TagBuilder("span");
			spanTime.InnerHtml = aComment.DateTimeStamp.ToString("MMM").ToUpper();
			
			divTimePad.InnerHtml += spanTime.ToString();
			divTimePad.InnerHtml += "&nbsp;";
			divTimePad.InnerHtml += aComment.DateTimeStamp.ToString("dd");
			
			divTime.InnerHtml += divTimePad.ToString();
			
			wrprDiv.InnerHtml += divTime.ToString();
			
			var clrDiv = new TagBuilder("div");
			clrDiv.MergeAttribute("class", "clear");
			clrDiv.InnerHtml += "&nbsp;";
			
			wrprDiv.InnerHtml += clrDiv.ToString();

            return wrprDiv.ToString(TagRenderMode.Normal);
        }

        public static string IssueReply(IssueReplyDetailsModel anIssueReply) {
        	var wrprDiv = new TagBuilder("div");
        	wrprDiv.MergeAttribute("class", "m-btm10");
        	
        	var profileDiv = new TagBuilder("div");
        	profileDiv.MergeAttribute("class", "col-3 center issue-profile");
        	
        	/* ALTER THIS FUNCTIONALITY TO DISPLAY THE PROFILE PICTURE AND USERNAME (HANDLE ANONYMOUS PROPERLY) */
            string myAvatarURL = "http://images.chron.com/photos/2008/05/19/graphic_defaultAvatar/graphic_defaultAvatar.jpg";
            string myUsername = "Anonymous";
            if (PrivacyHelper.IsAllowed(anIssueReply.IssueReply.User, PrivacyAction.DisplayProfile)) {
                myAvatarURL = PhotoHelper.ProfilePicture(anIssueReply.IssueReply.User);
                myUsername = anIssueReply.IssueReply.User.Username;
            }

        	var profileImg = new TagBuilder("img");
            profileImg.MergeAttribute("alt", myUsername);
            profileImg.MergeAttribute("src", myAvatarURL);
        	profileImg.MergeAttribute("class", "profile");


			profileDiv.InnerHtml += profileImg.ToString();
			wrprDiv.InnerHtml += profileDiv.ToString();
			
			var commentDiv = new TagBuilder("div");
			commentDiv.MergeAttribute("class", "m-lft col-18 m-rgt comment");
			
			var paddingDiv = new TagBuilder("div");
            paddingDiv.MergeAttribute("class", "p-a10");
			
			var spanSpeak = new TagBuilder("span");
            spanSpeak.MergeAttribute("class", "speak-lft");
			spanSpeak.InnerHtml = "&nbsp;";
			
			paddingDiv.InnerHtml += spanSpeak.ToString();
			
			var headTitle = new TagBuilder("h1");
            headTitle.MergeAttribute("class", "m-btm10");
			headTitle.InnerHtml += anIssueReply.IssueReply.Issue.Title;
			
			paddingDiv.InnerHtml += headTitle.ToString();
			paddingDiv.InnerHtml += anIssueReply.IssueReply.Issue.Description;
			
			var clrDiv = new TagBuilder("div");
			clrDiv.MergeAttribute("class", "clear");
			clrDiv.InnerHtml = "&nbsp;";
			
			paddingDiv.InnerHtml += clrDiv.ToString();
			commentDiv.InnerHtml += paddingDiv.ToString();
			
			wrprDiv.InnerHtml += commentDiv.ToString();
			
			var divTimeStamp = new TagBuilder("div");
        	divTimeStamp.MergeAttribute("class", "col-3 date-tile");
			
			var divTimePad = new TagBuilder("div");
			divTimePad.MergeAttribute("class", "p-a10");

			var spanTime = new TagBuilder("span");
			spanTime.InnerHtml = anIssueReply.IssueReply.Issue.DateTimeStamp.ToString("MMM").ToUpper();
			
			divTimePad.InnerHtml += spanTime.ToString();
			divTimePad.InnerHtml += "&nbsp;";
			divTimePad.InnerHtml += anIssueReply.IssueReply.Issue.DateTimeStamp.ToString("dd");
			
			divTimeStamp.InnerHtml += divTimePad.ToString();
			
			wrprDiv.InnerHtml += divTimeStamp.ToString();
			wrprDiv.InnerHtml += clrDiv.ToString();
			
			var replyDiv = new TagBuilder("div");
			replyDiv.MergeAttribute("class", "m-btm10");
			
			var rProfileDiv = new TagBuilder("div");
			rProfileDiv.MergeAttribute("class", "push-3 col-3 center issue-profile");
			
			var rProfileImg = new TagBuilder("img");
			rProfileImg.MergeAttribute("alt", "Reply Username");
			rProfileImg.MergeAttribute("src", "/Photos/no_profile_picture.jpg");
			rProfileImg.MergeAttribute("class", "profile");
			
			rProfileDiv.InnerHtml += rProfileImg.ToString();
			replyDiv.InnerHtml += rProfileDiv.ToString();
			
			var rCommentDiv = new TagBuilder("div");
            rCommentDiv.MergeAttribute("class", "push-3 m-lft col-15 m-rgt row");
			
			var rPaddingDiv = new TagBuilder("div");
			rPaddingDiv.MergeAttribute("class", "p-a10");
			
			rPaddingDiv.InnerHtml += spanSpeak.ToString();
			
			var rUserLink = new TagBuilder("a");
			rUserLink.MergeAttribute("class", "name");
			rUserLink.MergeAttribute("href", "#");
			rUserLink.InnerHtml = "Username";
			
			rPaddingDiv.InnerHtml += rUserLink.ToString();
			rPaddingDiv.InnerHtml += "&nbsp;";
			rPaddingDiv.InnerHtml += anIssueReply.IssueReply.Reply;
			
			rCommentDiv.InnerHtml += rPaddingDiv.ToString();
			
			replyDiv.InnerHtml += rCommentDiv.ToString();
			
			var rTimeStamp = new TagBuilder("div");
			rTimeStamp.MergeAttribute("class", "push-3 col-3 date-tile");
			
			var rTimePad = new TagBuilder("div");
			rTimePad.MergeAttribute("class", "p-a10");
			
			var rTime = new TagBuilder("span");
			rTime.InnerHtml = anIssueReply.IssueReply.DateTimeStamp.ToString("MMM").ToUpper();
			
			rTimePad.InnerHtml += rTime.ToString();
			rTimePad.InnerHtml += "&nbsp;";
			rTimePad.InnerHtml += anIssueReply.IssueReply.DateTimeStamp.ToString("dd");
			
			rTimeStamp.InnerHtml += rTimePad.ToString();
			
			replyDiv.InnerHtml += rTimeStamp.ToString();
			replyDiv.InnerHtml += clrDiv.ToString();

            return wrprDiv.ToString(TagRenderMode.Normal) + clrDiv.ToString(TagRenderMode.Normal) + replyDiv.ToString(TagRenderMode.Normal);
        }

        public static string BuildIssueDisplay(IEnumerable<IssueWithDispositionModel> anIssues, bool anIsLike) {
            string myIssueDisplay = string.Empty;

            foreach (IssueWithDispositionModel myIssue in anIssues) {
                string myAvatarURL = "http://images.chron.com/photos/2008/05/19/graphic_defaultAvatar/graphic_defaultAvatar.jpg";
                string myUsername = "Anonymous";
                if (PrivacyHelper.IsAllowed(myIssue.Issue.User, PrivacyAction.DisplayProfile)) {
                    myAvatarURL = "http://wedonetwork.co.uk/wedotech/wp-content/uploads/2010/08/master-chief-badass.jpg";
                    myUsername = myIssue.Issue.User.Username;
                }

                var myOuterDiv = new TagBuilder("div");
                myOuterDiv.MergeAttribute("class", "m-btm30");

                var myDivImageWrapper = new TagBuilder("div");
                myDivImageWrapper.MergeAttribute("class", "col-2 center m-rgt10");
                myDivImageWrapper.InnerHtml = "<img src=\"" + myAvatarURL + "\" alt=\"" + myUsername + "\" class=\"profile\"  />";
                myOuterDiv.InnerHtml = myDivImageWrapper.ToString();

                var myContextDiv = new TagBuilder("div");
                myContextDiv.MergeAttribute("class", "col-9");

                var myUserlink = new TagBuilder("a");
                myUserlink.MergeAttribute("class", "profile");
                myUserlink.MergeAttribute("href", "#");
                myUserlink.InnerHtml = myUsername;
                myContextDiv.InnerHtml += myUserlink.ToString();

                myContextDiv.InnerHtml += myIssue.Issue.Title;

                var mySpan = new TagBuilder("span");
                mySpan.MergeAttribute("class", "profile");
                mySpan.InnerHtml = "On %%POST DATE%% by " + myUsername + " - %%COUNTRY%% (%%STATE%%)";
                myContextDiv.InnerHtml += mySpan.ToString();

                /*
                var myContextSpan = new TagBuilder("span");
                myContextSpan.MergeAttribute("class", "teal2");
                myContextSpan.InnerHtml = myUsername + ": ";
                myContextDiv.InnerHtml = myContextSpan.ToString(); 
                myContextDiv.InnerHtml += anIssue.Title;
                */

                string myLike = new StringBuilder().AppendFormat("<a href=\"/Issue/IssueDisposition?issueId={0}&disposition={1}\" class=\"like\"><img src=\"/Content/images/like-sm.png\" alt=\"Like\" />Like</a>", myIssue.Issue.Id, Disposition.Like).ToString();
                myContextDiv.InnerHtml += myLike.ToString();

                string myDislike = new StringBuilder().AppendFormat("<a href=\"/Issue/IssueDisposition?issueId={0}&disposition={1}\" class=\"dislike\">Dislike<img src=\"/Content/images/dislike-sm.png\" alt=\"Dislike\" /></a>", myIssue.Issue.Id, Disposition.Dislike).ToString();
                myContextDiv.InnerHtml += myDislike.ToString();

                myOuterDiv.InnerHtml += myContextDiv.ToString();

                var myClearDiv = new TagBuilder("div");
                myClearDiv.MergeAttribute("class", "clear");
                myClearDiv.InnerHtml = "&nbsp;";
                myOuterDiv.InnerHtml += myClearDiv.ToString();

                myIssueDisplay += myOuterDiv.ToString();
            }

            return myIssueDisplay;
        }
    }
}
