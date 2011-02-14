using System;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace HaveAVoice.Helpers.UI {
    public class IssueHelper {
        public static string PersonFilterButton(PersonFilter aFilter, Dictionary<string, string> aSelectedFilters, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            return Filter("PersonFilter", "FilterIssueByPersonFilter", aFilter.ToString(), aSelectedFilters, aDisplayName, aNonSelectedCssClass, aSelectedCssClass);
        }

        public static string IssueStanceFilterButton(IssueStanceFilter aFilter, Dictionary<string, string> aSelectedFilters, string aNonSelectedCssClass, string aSelectedCssClass) {
            return Filter("IssueStanceFilter", "FilterIssueByIssueStanceFilter", aFilter.ToString(), aSelectedFilters, aFilter.ToString(), aNonSelectedCssClass, aSelectedCssClass);
        }

        private static string Filter(string aFilterType, string anActionMethodName, string aFilterText, Dictionary<string, string> aSelectedFilters, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            var linkTag = new TagBuilder("a");
            linkTag.InnerHtml = aDisplayName;
            linkTag.MergeAttribute("href", "/Issue/" + anActionMethodName + "?filterValue=" + aFilterText);
            string mySelectedFilter = aSelectedFilters[aFilterType];
            if (mySelectedFilter == aFilterText) {
                linkTag.MergeAttribute("class", aSelectedCssClass);
            } else {
                linkTag.MergeAttribute("class", aNonSelectedCssClass);
            }

            return linkTag.ToString();
        }

        public static string PersonFilterButton(PersonFilter aFilter, PersonFilter aSelectedFilter, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            var linkTag = new TagBuilder("a");
            linkTag.InnerHtml = aDisplayName;
            linkTag.MergeAttribute("href", "/Profile/FilterFeed?filterValue=" + aFilter);
            if (aSelectedFilter == aFilter) {
                linkTag.MergeAttribute("class", aSelectedCssClass);
            } else {
                linkTag.MergeAttribute("class", aNonSelectedCssClass);
            }

            return linkTag.ToString();
        }

        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            return BuildIssueReplyTable(anIssueReply);
        }

        private static string BuildIssueReplyTable(IssueReplyModel anIssueReply) {
            bool myIsAllowed = PrivacyHelper.IsAllowed(anIssueReply.User, PrivacyAction.DisplayProfile);

        	var stanceDiv = new TagBuilder("div");

            if(anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
        	    stanceDiv.MergeAttribute("class", "agree m-btm10");
            } else {
                stanceDiv.MergeAttribute("class", "disagree m-btm10");
            }

			var profileDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
			    profileDiv.MergeAttribute("class", "col-2 center push-20");
			} else {
				profileDiv.MergeAttribute("class", "push-6 col-2 center");
			}

			var profileImg = new TagBuilder("img");
			if (anIssueReply.Anonymous || !myIsAllowed) {
				profileImg.MergeAttribute("alt", "Anonymous");
                profileImg.MergeAttribute("src", PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE));
			}  else {
				profileImg.MergeAttribute("alt", NameHelper.FullName(anIssueReply.User));
				profileImg.MergeAttribute("src", PhotoHelper.ProfilePicture(anIssueReply.User));
			}
			profileImg.MergeAttribute("class", "profile");
			
			profileDiv.InnerHtml += profileImg.ToString();
			stanceDiv.InnerHtml += profileDiv.ToString();
			
			var stanceComment = new TagBuilder("div");
			if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
				stanceComment.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
			} else {
				stanceComment.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
			}
			
			var spanDirSpeak = new TagBuilder("span");
			
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
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
            if (anIssueReply.Anonymous || !myIsAllowed) {
				hrefName.InnerHtml = "Anonymous";
				hrefName.MergeAttribute("href", "#");
			} else {
                hrefName.InnerHtml = NameHelper.FullName(anIssueReply.User);
                hrefName.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
			}
			
			divCommentPad.InnerHtml += hrefName.ToString();
			divCommentPad.InnerHtml += "&nbsp;";
			divCommentPad.InnerHtml += anIssueReply.Reply;

			var clrDiv = new TagBuilder("div");
			clrDiv.MergeAttribute("class", "clear");
			clrDiv.InnerHtml += "&nbsp;";
			
			divCommentPad.InnerHtml += clrDiv.ToString();

			var optionWrpr = new TagBuilder("div");
			optionWrpr.MergeAttribute("class", "col-11 options");

            var optionsDiv = new TagBuilder("div");
            optionsDiv.MergeAttribute("class", "p-v10");

			var editDiv = new TagBuilder("div");
			editDiv.MergeAttribute("class", "col-2 center");
			
            /*UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();
            if (anIssueReply.User.Id == myUserInformationModel.Details.Id && HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Issue_Reply)) {
                var myEdit = new TagBuilder("a");
                myEdit.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReply.Id));
                myEdit.MergeAttribute("class", "edit");
                myEdit.InnerHtml += "Edit";
                editDiv.InnerHtml += myEdit.ToString();
            } else {*/
            	editDiv.InnerHtml += "&nbsp;";
            //}
            optionsDiv.InnerHtml += editDiv.ToString();
            
            var deleteDiv = new TagBuilder("div");
            deleteDiv.MergeAttribute("class", "col-3 center");
            
            /*if(anIssueReply.User.Id == myUserInformationModel.Details.Id && HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Issue_Reply)) {
                var myDelete = new TagBuilder("a");
                myDelete.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReply.Id));
                myDelete.MergeAttribute("class", "delete");
                myDelete.InnerHtml += "Delete";
                deleteDiv.InnerHtml += myDelete.ToString();
            } else {*/
            	deleteDiv.InnerHtml += "&nbsp;";
            //}
            optionsDiv.InnerHtml += deleteDiv.ToString();
            
            var likeDiv = new TagBuilder("div");
            likeDiv.MergeAttribute("class", "col-3 center");
            
            var dislikeDiv = new TagBuilder("div");
            dislikeDiv.MergeAttribute("class", "col-3 center");
            
            if(!anIssueReply.HasDisposition && HAVUserInformationFactory.IsLoggedIn()) {
                var myLikeDisposition = new TagBuilder("a");
                myLikeDisposition.MergeAttribute("href", LinkHelper.AgreeIssueReply(anIssueReply.Id, anIssueReply.Issue.Id, SiteSection.Issue, anIssueReply.Issue.Id));
                myLikeDisposition.MergeAttribute("class", "like");
                myLikeDisposition.InnerHtml += "Agree (" + anIssueReply.TotalAgrees + ")";
                likeDiv.InnerHtml +=myLikeDisposition.ToString();

                var myDislikeDisposition = new TagBuilder("a");
                myDislikeDisposition.MergeAttribute("href", LinkHelper.DisagreeIssueReply(anIssueReply.Id, anIssueReply.Issue.Id, SiteSection.Issue, anIssueReply.Issue.Id));
                myDislikeDisposition.MergeAttribute("class", "dislike");
                myDislikeDisposition.InnerHtml += "Disagree (" + anIssueReply.TotalDisagrees + ")";
                dislikeDiv.InnerHtml +=myDislikeDisposition.ToString();
            } else {
                var myLikeSpan = new TagBuilder("span");
                myLikeSpan.MergeAttribute("class", "like");
                string mySingleOrPlural = anIssueReply.TotalAgrees == 1 ? " Person Agrees" : " People Agree";
                myLikeSpan.InnerHtml = anIssueReply.TotalAgrees.ToString() + mySingleOrPlural;
                likeDiv.InnerHtml += myLikeSpan.ToString();

                var myDislikeSpan = new TagBuilder("span");
                myDislikeSpan.MergeAttribute("class", "dislike");
                mySingleOrPlural = anIssueReply.TotalDisagrees == 1 ? " Person Disagrees" : " People Disagree";
                myDislikeSpan.InnerHtml = anIssueReply.TotalDisagrees.ToString() + mySingleOrPlural;
                dislikeDiv.InnerHtml += myDislikeSpan.ToString();
            }

			optionsDiv.InnerHtml += likeDiv.ToString();
			optionsDiv.InnerHtml += dislikeDiv.ToString();
			
			optionsDiv.InnerHtml += clrDiv.ToString();
			
			optionWrpr.InnerHtml += optionsDiv.ToString();
			
			divCommentPad.InnerHtml += optionWrpr.ToString();
			
			stanceComment.InnerHtml += divCommentPad.ToString();

			var divTimeStamp = new TagBuilder("div");
			
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
			    divTimeStamp.MergeAttribute("class", "col-3 date-tile pull-9 right");
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
        	profileImg.MergeAttribute("alt", NameHelper.FullName(aComment.User));
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
            href.InnerHtml += NameHelper.FullName(aComment.User);
        	
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
        	
            string myAvatarURL = PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE);
            string myName = "Anonymous";
            if (PrivacyHelper.IsAllowed(anIssueReply.IssueReply.User, PrivacyAction.DisplayProfile)) {
                myAvatarURL = PhotoHelper.ProfilePicture(anIssueReply.IssueReply.User);
                myName = NameHelper.FullName(anIssueReply.IssueReply.User);
            }

        	var profileImg = new TagBuilder("img");
            profileImg.MergeAttribute("alt", myName);
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
			rUserLink.InnerHtml = "Full name";
			
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
                string myAvatarURL = PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE);
                string myName = "Anonymous";
                string myProfile = "/Authentication/Login";
                if (PrivacyHelper.IsAllowed(myIssue.Issue.User, PrivacyAction.DisplayProfile)) {
                    myAvatarURL = PhotoHelper.ProfilePicture(myIssue.Issue.User);
                    myName = NameHelper.FullName(myIssue.Issue.User);
                    myProfile = LinkHelper.Profile(myIssue.Issue.User);
                }
                
                var myClearDiv = new TagBuilder("div");
                myClearDiv.MergeAttribute("class", "clear");
                myClearDiv.InnerHtml = "&nbsp;";
                
                var myOuterDiv = new TagBuilder("div");
                myOuterDiv.MergeAttribute("class", "m-btm30");

                var myDivImageWrapper = new TagBuilder("div");
                myDivImageWrapper.MergeAttribute("class", "col-2 center m-rgt10");
                myDivImageWrapper.InnerHtml = "<img src=\"" + myAvatarURL + "\" alt=\"" + myName + "\" class=\"profile\"  />";
                myDivImageWrapper.InnerHtml += myClearDiv.ToString();
                myOuterDiv.InnerHtml = myDivImageWrapper.ToString();

                var myContextDiv = new TagBuilder("div");
                myContextDiv.MergeAttribute("class", "col-9");

                var myUserlink = new TagBuilder("a");
                myUserlink.MergeAttribute("class", "profile");
                myUserlink.MergeAttribute("href", myProfile);
                myUserlink.InnerHtml = myName;
                myContextDiv.InnerHtml += myUserlink.ToString();

				var issueLink = new TagBuilder("a");
				issueLink.MergeAttribute("class", "iss-fnt");
				issueLink.MergeAttribute("href", LinkHelper.IssueUrl(myIssue.Issue.Title));
				issueLink.InnerHtml = myIssue.Issue.Title;
                myContextDiv.InnerHtml += issueLink.ToString();
                myContextDiv.InnerHtml += "<br />";
                myContextDiv.InnerHtml += myIssue.Issue.Description + "  ";
                
                var readMore = new TagBuilder("a");
                readMore.MergeAttribute("class", "read-more");
                readMore.MergeAttribute("href", LinkHelper.IssueUrl(myIssue.Issue.Title));
                readMore.InnerHtml += "Read More and particiapte &raquo;";
                myContextDiv.InnerHtml += readMore.ToString();

                var mySpan = new TagBuilder("span");
                mySpan.MergeAttribute("class", "profile");
                mySpan.InnerHtml = String.Format("On {0} by {1} - {2}, {3}", myIssue.Issue.DateTimeStamp, myName, myIssue.Issue.City, myIssue.Issue.State);
                myContextDiv.InnerHtml += mySpan.ToString();
                myContextDiv.InnerHtml += myClearDiv.ToString();

                myOuterDiv.InnerHtml += myContextDiv.ToString();

                myOuterDiv.InnerHtml += myClearDiv.ToString();

                myIssueDisplay += myOuterDiv.ToString();
            }

            return myIssueDisplay;
        }
    }
}
