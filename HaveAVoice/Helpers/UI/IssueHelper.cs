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
		/* THIS FUNCTION NEEDS TO DETERMINE AGREE / DISAGREE OPPOSED TO PASSING A STYLE */
        //Henryk: Add IssueStance to IssueReplyModel#IssueStance
        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            return BuildIssueReplyTable(anIssueReply);
        }

		/* THIS FUNCTION NEEDS TO DETERMINE AGREE / DISAGREE OPPOSED TO PASSING A STYLE */
		/* NOTE: THE FUNCTIONS SHOUDL BE UPDATED TO ACCEPT A FILTER SET OPPOSED TO JUST USER / OFFICAL */
        //Henryk: I created an IssueFilter enum located in Helpers.Enums.
        public static string OfficialIssueReply(IssueReplyModel anIssueReply, IssueFilter aIssueFilter) {
            return BuildIssueReplyTable(anIssueReply);
        }

		/* AGREE / DISAGREE CAN BE AN ENUM OR BOOLEAN TO DETERMINE THE POST STANCE SO STRUCTURE CAN CHANGE */
		/* NOTE: I STRUCTURED IT TO DEFAULT TO AGREE / BUT MADE NOTES ON HOW IT SHOULD CHANGE*/
        //Henryk: Added if/else for aggree/disagree
        private static string BuildIssueReplyTable(IssueReplyModel anIssueReply) {
        	var stanceDiv = new TagBuilder("div");

        	/* THIS MERGE SHOULD BE BASED UPON THE VALUE PASSED RESULTING VALUE SHOULD RESOLVE TO CLASSES "agree" OR "disagree" */
            //henryk: donzo
            if(anIssueReply.IssueStance == (int)IssueStance.Agree) {
        	    stanceDiv.MergeAttribute("class", "agree");
            } else {
                stanceDiv.MergeAttribute("class", "disagree");
            }

			/* IF STANCE IS DISAGREE AN ADDITIONAL CLASS OF "push-15" NEEDS TO BE APPENDED TO THE PROFILE STYLE */
            //henryk: donzo
			var profileDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStance.Disagree) {
			    profileDiv.MergeAttribute("class", "col-3 center push-15"); /* HERE IS WHERE THE CLASS "push-15" NEEDS TO BE APPENDED */ //henryk: donzo
			} else {
				profileDiv.MergeAttribute("class", "col-3 center");
			}

			var profileImg = new TagBuilder("img");
			if (anIssueReply.Anonymous) {
				profileImg.MergeAttribute("alt", "Anonymous");
				profileImg.MergeAttribute("src", "/Photos/no_profile_picture.jpg");
			}  else {
				profileImg.MergeAttribute("alt", anIssueReply.User.Username);
				profileImg.MergeAttribute("src", PhotoHelper.ProfilePicture(anIssueReply.User)); /* NOTE: I DON'T KNOW IF THIS IS THE CORRECT CALL (IF NO PLEASE CORRECT)... Henryk: No problem I created a PhotoHelper#ProfilePicture, but the ProfilePicture should be in the model itself... I will change this later but it's good like this for now */
			}
			profileImg.MergeAttribute("class", "profile");
			
			profileDiv.InnerHtml += profileImg.ToString();
			stanceDiv.InnerHtml += profileDiv.ToString();
			
			var stanceComment = new TagBuilder("div");
			stanceComment.MergeAttribute("class", "m-lft col-12 m-rgt comment");
			
			var spanDirSpeak = new TagBuilder("span");
			
			/* NOTE: IF STANCE IS AGREE CLASS SHOULD BE "speak-lft" IF STANCE IS DISAGREE CLASS SHOULD BE "speak-rgt" */
            //Henryk: done
            if (anIssueReply.IssueStance == (int)IssueStance.Agree) {
                spanDirSpeak.MergeAttribute("class", "speak-lft");
            } else {
                spanDirSpeak.MergeAttribute("class", "speak-rgt");
            }
			spanDirSpeak.InnerHtml = "&nbsp;";
			
			stanceComment.InnerHtml += spanDirSpeak.ToString();

			var divCommentPad = new TagBuilder("div");
			divCommentPad.MergeAttribute("class","p-a10"); //Henryk: This had only one parameter, I add "class" as the first one, is this right?
			
			var hrefName = new TagBuilder("a");
			hrefName.MergeAttribute("class", "name");
			if (anIssueReply.Anonymous)
			{
				hrefName.InnerHtml = "Anonymous";
				hrefName.MergeAttribute("href", "#");
			} else {
				hrefName.InnerHtml = anIssueReply.User.Username;
                hrefName.MergeAttribute("href", LinkHelper.ProfilePage(anIssueReply.User));
				/* THIS SHOULD LINK TO THE USER PROFILE */
                //Henryk: created LinkHelper to construct URL.
			}
			
			divCommentPad.InnerHtml += hrefName.ToString();
			divCommentPad.InnerHtml += anIssueReply.Reply;
			
			stanceComment.InnerHtml += divCommentPad.ToString();

			var divTimeStamp = new TagBuilder("div");
			
			/* IF STANCE IS DISAGREE ADDITIONAL CLASS OF "pull-15" NEEDS TO BE APPENDED TO THE TIMESTAMP STYLE */
            if (anIssueReply.IssueStance == (int)IssueStance.Disagree) {
			    divTimeStamp.MergeAttribute("class", "col-3 date-tile pull-15"); /* HERE IS WHERE THE CLASS "pull-15" NEEDS TO BE APPENDED */
                //Henryk: there was one parameter so I made the first parameter "class"...
            } else {
            	divTimeStamp.MergeAttribute("class", "col-3 date-tile");
            }
            
            stanceDiv.InnerHtml += stanceComment.ToString();
			
			var divTimePad = new TagBuilder("div");
			divTimePad.MergeAttribute("class", "p-a10");
			//Henryk: there was one parameter so I made the first parameter "class"...

			var spanTime = new TagBuilder("span");
			spanTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();
			
			divTimePad.InnerHtml += spanTime.ToString();
			divTimePad.InnerHtml += "&nbsp;";
			divTimePad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");
			
			stanceDiv.InnerHtml += divTimePad.ToString();
        
        	return stanceDiv.ToString(TagRenderMode.Normal);
        
			/*
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "400px");

            var trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            if(anIssueReply.Anonymous) {
                tdTag.InnerHtml = "<strong>Anonymous</strong>";
            } else {
                tdTag.InnerHtml = "<strong>" + anIssueReply.User.Username + "</strong>";
            }
            tdTag.InnerHtml += " @ " + anIssueReply.DateTimeStamp;
            trTag.InnerHtml += tdTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.MergeAttribute("style", "text-align:right;");
            tdTag.InnerHtml = "<strong>" + anIssueReply.User.City + ", " + anIssueReply.User.State + "</strong>";
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.MergeAttribute("colspan", "2");
            tdTag.InnerHtml = anIssueReply.Reply;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.InnerHtml = String.Format("<a href=\"/IssueReply/View/{0}\">{1}</a>", 
                anIssueReply.Id, 
                "Comments ( " + anIssueReply.CommentCount + " )");
            trTag.InnerHtml += tdTag.ToString();
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.InnerHtml = ComplaintHelper.IssueReplyLink(anIssueReply.Id);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
            */
            
            /* NOTE: PLEASE CHECK FOR ANY SYNTAX ERRORS AND NOTICE I CURRENTLY LEFT OFF THE COMMENT / LIKE / DISLIKE / REPORT / EDIT LINKS UNTIL I FIGURE HOW TO FIT THESE IN */
            /* CLASSES WILL BE CHANGED, BUT THE STRUCTURE IS IN PLACE TO GIVE YOU AN IDEA OF THE STYLE ONCE COMPILED. */
        }

        public static string Comment(IssueReplyComment aComment) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "400px");

            var trTag = new TagBuilder("tr");
            string rowStyle = "text-align:left; background-color:#6699CC; color:Black";
            trTag.MergeAttribute("style", rowStyle);

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = "<strong>" + aComment.User.Username + "</strong>";
            tdTag.InnerHtml += " @ " + aComment.DateTimeStamp;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = aComment.Comment;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("style", "text-align:right;");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = ComplaintHelper.IssueReplyCommentLink(aComment.Id);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        public static string IssueReply(IssueReplyDetailsModel anIssueReply) {
        	var wrprDiv = new TagBuilder("div");
        	wrprDiv.MergeAttribute("class", "m-btm10");
        	
        	var profileDiv = new TagBuilder("div");
        	profileDiv.MergeAttribute("class", "col-3 center issue-profile");
        	
        	/* ALTER THIS FUNCTIONALITY TO DISPLAY THE PROFILE PICTURE AND USERNAME (HANDLE ANONYMOUS PROPERLY) */
        	var profileImg = new TagBuilder("img");
        	profileImg.MergeAttribute("alt", "Username");
        	profileImg.MergeAttribute("src", "/Photos/no_profile_picture.jpg");
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
