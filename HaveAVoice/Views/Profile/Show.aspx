<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <div class="col-21">
    <% Html.RenderPartial("Message"); %>

    <% if (Model.NavigationModel.SiteSection == SiteSection.MyProfile) { %>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>

        <% while(myNextFeedItem != FeedItem.None) { %>
            <% if(myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
                <% ViewData["Count"] = cnt; %>
                issue
                <% Html.RenderPartial("IssueFeed", myIssue); %>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) {%>
                <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
                <% ViewData["Count"] = cnt; %>
                issue reply
                <% Html.RenderPartial("IssueReplyFeed", myIssueReply); %>
            <% } else if (myNextFeedItem == FeedItem.Photo) {%>
                <% PhotoAlbumFeedModel myPhotoAlbum = Model.Model.GetNextPhotoAlbum(); %>
            <% } %>
            <% cnt++; %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
        <% } %>
    <% } else if (Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>
        <% while(myNextFeedItem != FeedItem.None) { %>
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
