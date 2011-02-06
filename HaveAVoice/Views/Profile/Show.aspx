﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		<!--
			jQuery(function() {
				$("a[rel=post]").click(function() {
					if ($("div.post").is(":visible")) {
						$("div.post").slideUp("fast");
					} else {
						$("div.post").slideDown("fast");
					}
					return false;
				});
			});
		//-->
	</script>
    
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <div class="col-21">

    <div class="fnt-10 m-btm5">
        <% if (Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
    	    <div class="m-lft col-3 m-rgt center">
    		    <a href="#" rel="post" class="filter">Post on Board</a>
    		    <div class="clear">&nbsp;</div>
    	    </div>

		    <div class="m-lft col-2 m-rgt f-rgt center">
			    <a href="#" class="filter dislike">Report</a>
			    <div class="clear">&nbsp;</div>
		    </div>
        <% } else { %>
        <% TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA] = TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA]; %>
        <% PersonFilter myFilter = (PersonFilter)TempData[HAVConstants.FILTER_TEMP_DATA];  %>
            <div class="m-lft col-2 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.PoliticalCandidates, myFilter, "Political Candidates", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-2 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.Politicians, myFilter, "Politicians", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-2 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.People, myFilter, "People", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
        <% } %>
    </div>

    <div class="clear">&nbsp;</div>
    <% Html.RenderPartial("Message"); %>
    
    <% if (Model.NavigationModel.SiteSection == SiteSection.MyProfile) { %>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>

        <% while(myNextFeedItem != FeedItem.None) { %>
            <% ViewData["SiteSection"] = Model.NavigationModel.SiteSection; %>
            <% ViewData["SourceId"] = Model.NavigationModel.User.Id; %>
            <% if(myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
                <% ViewData["Count"] = cnt; %>
                <% Html.RenderPartial("IssueFeed", myIssue); %>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) {%>
                <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
                <% ViewData["Count"] = cnt; %>
                <% Html.RenderPartial("IssueReplyFeed", myIssueReply); %>
            <% } else if (myNextFeedItem == FeedItem.Photo) {%>
                <% PhotoAlbumFeedModel myPhotoAlbum = Model.Model.GetNextPhotoAlbum(); %>
            <% } %>
            <% cnt++; %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
        <% } %>
    <% } else if (Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
	    <div class="post m-btm5 m-top5">
		    <% using (Html.BeginForm("Create", "Board", new { sourceUserId = Model.NavigationModel.User.Id })) { %>
		    <div class="row">
			    <div class="push-2 col-2 center">
	                <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
				    <img src="<%= myUserInfo.ProfilePictureUrl  %>" alt="<%= myUserInfo.FullName %>" class="profile" />
			    </div>
			    <div class="push-2 m-lft col-14 comment">
				    <span class="speak-lft">&nbsp;</span>
				    <div class="p-a10">
					    <%= Html.ValidationMessage("BoardMessage", "*")%>
					    <%= Html.TextArea("BoardMessage", (string)TempData["BoardMessage"], 5, 63, new { resize = "none", @class = "comment" })%>
					    <div class="clear">&nbsp;</div>
					    <hr />
					    <div class="col-13 right">
						    <input type="submit" value="Submit" class="button" />
						    <input type="button" value="Clear" class="button" />
					    </div>
					    <div class="clear">&nbsp;</div>
				    </div>
			    </div>
			    <div class="clear">&nbsp;</div>
		    </div>
		    <div class="clear">&nbsp;</div>
		    <% } %>
	    </div>
	    <div class="clear">&nbsp;</div>
        
        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>
        <% while(myNextFeedItem != FeedItem.None) { %>
            <% ViewData["SiteSection"] = Model.NavigationModel.SiteSection; %>
            <% ViewData["SourceId"] = Model.NavigationModel.User.Id; %>
            <% if (myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
                <% ViewData["Count"] = cnt; %>
                <% Html.RenderPartial("IssueFeed", myIssue); %>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) { %>
                <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
                <% ViewData["Count"] = cnt; %>
                <% Html.RenderPartial("IssueReplyFeed", myIssueReply); %>
            <% }  else if (myNextFeedItem == FeedItem.Photo) { %>
                <% PhotoAlbumFeedModel myPhotoAlbum = Model.Model.GetNextPhotoAlbum(); %>
            <% } else if (myNextFeedItem == FeedItem.Board) { %>
                <% BoardFeedModel myBoard = Model.Model.GetNextBoard(); %>
                <% ViewData["Count"] = cnt; %>
                <% Html.RenderPartial("BoardFeed", myBoard); %>
            <% } %>
            <% cnt++; %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
        <% } %>
    <% } %>
	</div>
</asp:Content>
