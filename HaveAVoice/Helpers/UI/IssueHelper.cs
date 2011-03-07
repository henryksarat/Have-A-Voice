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
        	var myReplyDiv = new TagBuilder("div");

            if(anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
        	    myReplyDiv.MergeAttribute("class", "agree m-btm10");
            } else {
                myReplyDiv.MergeAttribute("class", "disagree m-btm10");
            }

			var myProfileDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
			    myProfileDiv.MergeAttribute("class", "col-2 center push-20");
			} else {
				myProfileDiv.MergeAttribute("class", "push-6 col-2 center");
			}

			var myProfileImage = new TagBuilder("img");
			if (anIssueReply.Anonymous) {
				myProfileImage.MergeAttribute("alt", "Anonymous");
                myProfileImage.MergeAttribute("src", PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE));
			}  else {
				myProfileImage.MergeAttribute("alt", NameHelper.FullName(anIssueReply.User));
				myProfileImage.MergeAttribute("src", PhotoHelper.ProfilePicture(anIssueReply.User));
			}
			myProfileImage.MergeAttribute("class", "profile");
			
			myProfileDiv.InnerHtml += myProfileImage.ToString();
			myReplyDiv.InnerHtml += myProfileDiv.ToString();
			
			var myReplyCommentDiv = new TagBuilder("div");
			if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
				myReplyCommentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
			} else {
				myReplyCommentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
			}
			
			var mySpeakSpan = new TagBuilder("span");
			
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                mySpeakSpan.MergeAttribute("class", "speak-lft");
            } else {
                mySpeakSpan.MergeAttribute("class", "speak-rgt");
            }
			mySpeakSpan.InnerHtml = "&nbsp;";
			
			myReplyCommentDiv.InnerHtml += mySpeakSpan.ToString();

			var myReplyCommentPad = new TagBuilder("div");
			myReplyCommentPad.MergeAttribute("class","p-a10");
			
			var myName = new TagBuilder("a");
			myName.MergeAttribute("class", "name");
            if (anIssueReply.Anonymous) {
				myName.InnerHtml = "Anonymous";
				myName.MergeAttribute("href", "#");
			} else {
                myName.InnerHtml = NameHelper.FullName(anIssueReply.User);
                myName.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
			}
			
			myReplyCommentPad.InnerHtml += myName.ToString();
			myReplyCommentPad.InnerHtml += "&nbsp;";
			myReplyCommentPad.InnerHtml += anIssueReply.Reply;

            myReplyCommentPad.InnerHtml += ClearDiv();

            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

			var myEditAndStancesDiv = new TagBuilder("div");
			myEditAndStancesDiv.MergeAttribute("class", "col-11 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.MergeAttribute("class", "p-v10");

            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myAgreeDiv = AgreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalAgrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn());
            var myDisagreeDiv = DisagreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalDisagrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn());

            myEditAndStancesPad.InnerHtml += myDeleteDiv.ToString();
            myEditAndStancesPad.InnerHtml += myEditDiv.ToString();
			myEditAndStancesPad.InnerHtml += myAgreeDiv.ToString();
			myEditAndStancesPad.InnerHtml += myDisagreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += ClearDiv();
			
			myEditAndStancesDiv.InnerHtml += myEditAndStancesPad.ToString();
			
			myReplyCommentPad.InnerHtml += myEditAndStancesDiv.ToString();
			
			myReplyCommentDiv.InnerHtml += myReplyCommentPad.ToString();

			var divTimeStamp = new TagBuilder("div");
			
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
			    divTimeStamp.MergeAttribute("class", "col-3 date-tile pull-9 right");
            } else {
            	divTimeStamp.MergeAttribute("class", "col-3 date-tile push-6");
            }
            
            myReplyDiv.InnerHtml += myReplyCommentDiv.ToString();
			
			var divTimePad = new TagBuilder("div");
			divTimePad.MergeAttribute("class", "p-a10");

			var spanTime = new TagBuilder("span");
			spanTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();
			
			divTimePad.InnerHtml += spanTime.ToString();
			divTimePad.InnerHtml += "&nbsp;";
			divTimePad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");
			
			divTimeStamp.InnerHtml += divTimePad.ToString();
			
			myReplyDiv.InnerHtml += divTimeStamp.ToString();
			myReplyDiv.InnerHtml += ClearDiv();

        	return myReplyDiv.ToString(TagRenderMode.Normal);
        }

        public static string Comment(IssueReplyComment aComment) {
			var clrDiv = new TagBuilder("div");
			clrDiv.MergeAttribute("class", "clear");
			clrDiv.InnerHtml += "&nbsp;";
			
        	var wrprDiv = new TagBuilder("div");
        	wrprDiv.MergeAttribute("class", "m-btm5 alt");
        	
        	var profileDiv = new TagBuilder("div");
        	profileDiv.MergeAttribute("class", "push-6 col-3 center");

            var profileImg = new TagBuilder("img");
        	profileImg.MergeAttribute("class", "profile");
        	profileImg.MergeAttribute("alt", NameHelper.FullName(aComment.User));
        	profileImg.MergeAttribute("src", PhotoHelper.ProfilePicture(aComment.User));
        	
        	profileDiv.InnerHtml += profileImg.ToString();
        	wrprDiv.InnerHtml += profileDiv.ToString();
        	
        	var commentDiv = new TagBuilder("div");
        	commentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
        	
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
        	
        	var divOptions = new TagBuilder("div");
        	divOptions.MergeAttribute("class", "options p-v10");
        	
        	var divReport = new TagBuilder("div");
        	divReport.MergeAttribute("class", "col-1 push-10");
        	
        	divReport.InnerHtml += ComplaintHelper.IssueReplyCommentLink(aComment.Id);
        	divReport.InnerHtml += clrDiv.ToString();
        	
        	divOptions.InnerHtml += divReport.ToString();
        	
        	paddingDiv.InnerHtml += divOptions.ToString();

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
			
			wrprDiv.InnerHtml += clrDiv.ToString();

            return wrprDiv.ToString(TagRenderMode.Normal);
        }

        public static string IssueReply(IssueReply anIssueReply) {
            string myIssueDiv = BuildIssueForIssueReplyDisplay(anIssueReply.Issue);
            string myIssueReplyDiv = BuildReplyForIssueReplyDisplay(anIssueReply);

            return myIssueDiv + ClearDiv() + myIssueReplyDiv;
        }

        private static string BuildIssueForIssueReplyDisplay(Issue anIssue) {
            var myIssueDiv = new TagBuilder("div");
            myIssueDiv.MergeAttribute("class", "m-btm10");

            var myIssueProfileDiv = new TagBuilder("div");
            myIssueProfileDiv.MergeAttribute("class", "col-3 center issue-profile");

            string myIssueProfilePictureURL = PhotoHelper.ProfilePicture(anIssue.User);
            string myIssueFullName = NameHelper.FullName(anIssue.User);

            var myProfileImg = new TagBuilder("img");
            myProfileImg.MergeAttribute("alt", myIssueFullName);
            myProfileImg.MergeAttribute("src", myIssueProfilePictureURL);
            myProfileImg.MergeAttribute("class", "profile");

            myIssueProfileDiv.InnerHtml += myProfileImg.ToString();
            myIssueDiv.InnerHtml += myIssueProfileDiv.ToString();

            var myIssueInfoDiv = new TagBuilder("div");
            myIssueInfoDiv.MergeAttribute("class", "m-lft col-18 m-rgt comment");

            var myIssueInfoPadding = new TagBuilder("div");
            myIssueInfoPadding.MergeAttribute("class", "p-a10");
            myIssueInfoPadding.InnerHtml += InfoSpeakSpan();

            var myHeadTitle = new TagBuilder("h1");
            var myIssueLink = new TagBuilder("a");
            myIssueLink.MergeAttribute("href", LinkHelper.IssueUrl(anIssue.Title));
            myIssueLink.InnerHtml = anIssue.Title;
            myHeadTitle.InnerHtml += myIssueLink.ToString();

            myIssueInfoPadding.InnerHtml += myHeadTitle.ToString();
            myIssueInfoPadding.InnerHtml += anIssue.Description;

            myIssueInfoPadding.InnerHtml += ClearDiv();
            myIssueInfoDiv.InnerHtml += myIssueInfoPadding.ToString();

            myIssueDiv.InnerHtml += myIssueInfoDiv.ToString();

            var myIssueTimeStamp = new TagBuilder("div");
            myIssueTimeStamp.MergeAttribute("class", "col-3 date-tile");

            var myIssueTimeStampPad = new TagBuilder("div");
            myIssueTimeStampPad.MergeAttribute("class", "p-a10");

            var myTimeStampSpan = new TagBuilder("span");
            myTimeStampSpan.InnerHtml = anIssue.DateTimeStamp.ToString("MMM").ToUpper();

            myIssueTimeStampPad.InnerHtml += myTimeStampSpan.ToString();
            myIssueTimeStampPad.InnerHtml += "&nbsp;";
            myIssueTimeStampPad.InnerHtml += anIssue.DateTimeStamp.ToString("dd");

            myIssueTimeStamp.InnerHtml += myIssueTimeStampPad.ToString();

            myIssueDiv.InnerHtml += myIssueTimeStamp.ToString();
            myIssueDiv.InnerHtml += ClearDiv();

            return myIssueDiv.ToString();
        }

        private static string BuildReplyForIssueReplyDisplay(IssueReply anIssueReply) {
            var myReplyDiv = new TagBuilder("div");
            myReplyDiv.MergeAttribute("class", "m-btm10 alt");

            var myReplyProfileDiv = new TagBuilder("div");
            myReplyProfileDiv.MergeAttribute("class", "push-3 col-3 center issue-profile");

            string myReplyProfilePictureURL = PhotoHelper.ProfilePicture(anIssueReply.User);
            string myReplyFullName = NameHelper.FullName(anIssueReply.User);

            var myProfileImage = new TagBuilder("img");
            myProfileImage.MergeAttribute("alt", myReplyFullName);
            myProfileImage.MergeAttribute("src", myReplyProfilePictureURL);
            myProfileImage.MergeAttribute("class", "profile");

            myReplyProfileDiv.InnerHtml += myProfileImage.ToString();
            myReplyDiv.InnerHtml += myReplyProfileDiv.ToString();

            var myReplyInfoDiv = new TagBuilder("div");
            myReplyInfoDiv.MergeAttribute("class", "push-3 m-lft col-15 m-rgt comment");

            var myReplyInfoPadding = new TagBuilder("div");
            myReplyInfoPadding.MergeAttribute("class", "p-a10");
            myReplyInfoPadding.InnerHtml += InfoSpeakSpan();

            var myUserLink = new TagBuilder("a");
            myUserLink.MergeAttribute("class", "name");
            myUserLink.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
            myUserLink.InnerHtml = myReplyFullName;

            myReplyInfoPadding.InnerHtml += myUserLink.ToString();
            myReplyInfoPadding.InnerHtml += "&nbsp;";
            myReplyInfoPadding.InnerHtml += anIssueReply.Reply;
            myReplyInfoPadding.InnerHtml += ClearDiv();

            //asdasd
            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

            var myEditAndStancesDiv = new TagBuilder("div");
            myEditAndStancesDiv.MergeAttribute("class", "col-11 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.MergeAttribute("class", "p-v10");

            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);

            myEditAndStancesPad.InnerHtml += myDeleteDiv.ToString();
            myEditAndStancesPad.InnerHtml += myEditDiv.ToString();
            myEditAndStancesPad.InnerHtml += ClearDiv();

            myEditAndStancesDiv.InnerHtml += myEditAndStancesPad.ToString();

            myReplyInfoPadding.InnerHtml += myEditAndStancesDiv.ToString();

            myReplyInfoDiv.InnerHtml += myReplyInfoPadding.ToString();

            myReplyDiv.InnerHtml += myReplyInfoDiv.ToString();

            var myReplyTimeStampDiv = new TagBuilder("div");
            myReplyTimeStampDiv.MergeAttribute("class", "push-3 col-3 date-tile");

            var myReplyTimeStampPad = new TagBuilder("div");
            myReplyTimeStampPad.MergeAttribute("class", "p-a10");

            var rTime = new TagBuilder("span");
            rTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();

            myReplyTimeStampPad.InnerHtml += rTime.ToString();
            myReplyTimeStampPad.InnerHtml += "&nbsp;";
            myReplyTimeStampPad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");

            myReplyTimeStampDiv.InnerHtml += myReplyTimeStampPad.ToString();

            myReplyDiv.InnerHtml += myReplyTimeStampDiv.ToString();
            myReplyDiv.InnerHtml += ClearDiv();

            return myReplyDiv.ToString();
        }

        private static TagBuilder EditDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myEditDiv = new TagBuilder("div");
            myEditDiv.MergeAttribute("class", "col-2 center");

            if (IssueReplyHelper.ShouldDisplayEditLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myEditLink = new TagBuilder("a");
                myEditLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myEditLink.MergeAttribute("class", "edit");
                myEditLink.InnerHtml += "Edit";
                myEditDiv.InnerHtml += myEditLink.ToString();
            } else {
                myEditDiv.InnerHtml += "&nbsp;";
            }
            return myEditDiv;
        }

        private static TagBuilder DeleteDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myDeleteDiv = new TagBuilder("div");
            myDeleteDiv.MergeAttribute("class", "col-3 center");

            if (IssueReplyHelper.ShouldDisplayDeleteLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myDeleteLink = new TagBuilder("a");
                myDeleteLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myDeleteLink.MergeAttribute("class", "delete");
                myDeleteLink.InnerHtml += "Delete";
                myDeleteDiv.InnerHtml += myDeleteLink.ToString();
            } else {
                myDeleteDiv.InnerHtml += "&nbsp;";
            }
            return myDeleteDiv;
        }

        private static TagBuilder AgreeDiv(int anIssueReplyId, int anIssueId, int aTotalAgrees, bool aHasDisposition, bool aIsLoggedIn) {
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn,
               LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId),
               "like", aTotalAgrees, "Agrees", "Agree");  
        }

        private static TagBuilder DisagreeDiv(int anIssueReplyId, int anIssueId, int aTotalDisagrees, bool aHasDisposition, bool aIsLoggedIn) {
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn, 
                LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId), 
                "dislike", aTotalDisagrees, "Disagrees", "Disagree");  
        }

        private static TagBuilder StanceDiv(int anIssueReplyId, int anIssueId, bool aHasDisposition, bool aIsLoggedIn, 
                                            string aStanceUrl, string aLinkCssClass, int aTotalForStance, string aSingularDisplayText, string aPluralDisplayText) {
            var myStanceDiv = new TagBuilder("div");
            myStanceDiv.MergeAttribute("class", "col-3 center");

            if (!aHasDisposition && aIsLoggedIn) {
                var myDisagreeLink = new TagBuilder("a");
                myDisagreeLink.MergeAttribute("href", LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId));
                myDisagreeLink.MergeAttribute("class", aLinkCssClass);
                myDisagreeLink.InnerHtml += aPluralDisplayText + " (" + aTotalForStance + ")";
                myStanceDiv.InnerHtml += myDisagreeLink.ToString();
            } else {
                var myDisagreeSpan = new TagBuilder("span");
                myDisagreeSpan.MergeAttribute("class", aLinkCssClass);
                string mySingleOrPlural = aTotalForStance == 1 ? " Person " + aSingularDisplayText : " People " + aPluralDisplayText;
                myDisagreeSpan.InnerHtml = aTotalForStance.ToString() + mySingleOrPlural;
                myStanceDiv.InnerHtml += myDisagreeSpan.ToString();
            }

            return myStanceDiv;
        }

        private static string InfoSpeakSpan() {
            var myInfoSpeakSpan = new TagBuilder("span");
            myInfoSpeakSpan.MergeAttribute("class", "speak-lft");
            myInfoSpeakSpan.InnerHtml = "&nbsp;";
            return myInfoSpeakSpan.ToString();
        }

        private static string ClearDiv() {
            var myClearDiv = new TagBuilder("div");
            myClearDiv.MergeAttribute("class", "clear");
            myClearDiv.InnerHtml = "&nbsp;";
            return myClearDiv.ToString();
        }

        public static string BuildIssueDisplay(IEnumerable<IssueWithDispositionModel> anIssues, bool anIsLike) {
            string myIssueDisplay = string.Empty;

            foreach (IssueWithDispositionModel myIssue in anIssues) {
                string myAvatarURL = PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE);
                string myName = "Anonymous";
                string myProfile = "/Authentication/Login";
                //if (PrivacyHelper.IsAllowed(myIssue.Issue.User, PrivacyAction.DisplayProfile)) {
                    myAvatarURL = PhotoHelper.ProfilePicture(myIssue.Issue.User);
                    myName = NameHelper.FullName(myIssue.Issue.User);
                    myProfile = LinkHelper.Profile(myIssue.Issue.User);
                //}
                
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

        public static bool ShouldDisplayEditLink(UserInformationModel aUserInformation, Issue anIssue) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Issue) && aUserInformation.Details.Id == anIssue.UserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Any_Issue);
        }

        public static bool ShouldDisplayDeleteLink(UserInformationModel aUserInformation, Issue anIssue) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Issue) && aUserInformation.Details.Id == anIssue.UserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Any_Issue);
        }
    }
}
