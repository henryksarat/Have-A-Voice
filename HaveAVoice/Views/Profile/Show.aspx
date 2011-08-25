<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Helpers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		<!--
			jQuery(function() {
				$("div.reply-wrpr").each(function() {
					var total = $("div.reply", $(this)).not(":last").length;
					if (total > 1) {
						$("div.reply", $(this)).not(":last").css({ "display": "none" });
						$("div.reply:last", $(this)).parent().append("<a href=\"#\" class=\"m-btm5 push-3 alpha col-15 omega filter center\" rel=\"view-all\">View All (" + total + ")<div class=\"clear\">&nbsp;</div></a><div class=\"clear\">&nbsp;</div>");
					}
				});
				$("div.board-wrpr").each(function() {
					var total = $("div[class*=board-]", $(this)).not(":last").length;
					if (total > 1) {
						$("div[class*=board-]", $(this)).not(":last").css({ "display": "none" });
						$("div[class*=board-]:last", $(this)).parent().append("<a href=\"#\" class=\"m-btm5 push-3 alpha col-15 omega filter center\" rel=\"board-all\">View All (" + total + ")<div class=\"clear\">&nbsp;</div></a><div class=\"clear\">&nbsp;</div>");
					}
				});
				$("a[rel=post]").click(function() {
					if ($("div.post").is(":visible")) {
						$("div.post").slideUp("fast");
					} else {
						$("div.post").slideDown("fast");
					}
					return false;
				});
				$("a[rel=reply]").click(function() {
					$(this).fadeOut("fast", function() {
						$(this).next("div").slideDown("fast");
					});
					return false;
				});
				$("a[rel=view-all]").live("click", function() {
					$(this).fadeOut("fast", function() {
						$(this).parent("div.reply-wrpr").children("div.reply").slideDown("fast");
					});
					return false;
				});
				$("a[rel=board-all]").live("click", function() {
					$(this).fadeOut("fast", function() {
						$(this).parent("div.board-wrpr").children("div.board-row, div.board-alt").slideDown("fast");
					});
					return false;
				});
			});
		//-->
	</script>
    <% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
    
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
    </div>

    <div class="col-21">
    <% if (!PrivacyHelper.IsAllowed(Model.NavigationModel.User, PrivacyAction.DisplayProfile)) { %>
	    <div class="center fnt-14 color-4 bold">
		    This person's privacy settings prohibit you from viewing their profile.
		    <div class="clear">&nbsp;</div>
	    </div>
    <% } %>

    <div class="fnt-10 m-btm5">
        <% if (myUserInfo != null && Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
            <% if (PrivacyHelper.IsAllowed(Model.NavigationModel.User, PrivacyAction.DisplayProfile)) { %>
    	        <div class="m-lft col-3 m-rgt center">
    		        <a href="#" rel="post" class="filter">Post on Board</a>
    		        <div class="clear">&nbsp;</div>
    	        </div>
            <% } %>
		    <div class="m-lft col-3 m-rgt f-rgt center">
			    <a href="<%= LinkHelper.Report(Model.NavigationModel.User.Id, ComplaintType.ProfileComplaint) %>" class="filter dislike">Report Profile</a>
			    <div class="clear">&nbsp;</div>
		    </div>
        <% } else if(myUserInfo != null) { %>
        <% TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA] = TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA]; %>
        <% PersonFilter myFilter = (PersonFilter)TempData[HAVConstants.FILTER_TEMP_DATA];  %>
            <div class="m-lft col-3 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.PoliticalCandidates, myFilter, "Political Candidates", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-3 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.Politicians, myFilter, "Elected Officials", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-2 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.People, myFilter, "Friends", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-2 m-rgt f-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.All, myFilter, "All", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
		    </div>
        <% } %>
        <div class="clear">&nbsp;</div>
    </div>

    <div class="clear">&nbsp;</div>
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>
    <% if (Model.NavigationModel.SiteSection == SiteSection.MyProfile) { %>
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
        <% if (PrivacyHelper.IsAllowed(Model.NavigationModel.User, PrivacyAction.DisplayProfile)) { %>
            <% if (myUserInfo != null) { %>
	            <div class="post m-btm5 m-top5">
		            <% using (Html.BeginForm("Create", "Board", new { sourceUserId = Model.NavigationModel.User.Id })) { %>
		            <div class="row">
			            <div class="push-2 col-2 center">
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
            <% } %>
            <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
            <% int cnt = 0; %>
            <% while (myNextFeedItem != FeedItem.None) { %>
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
                <% } else if (myNextFeedItem == FeedItem.Photo) { %>
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
    <% } %>
	</div>
</asp:Content>
