<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	IssueActivity
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>

    <div class="col-21">
        <div class="action-bar bold p-a10 m-btm20 color-4">
        	Issues Participating In
        </div>

    <% Html.RenderPartial("Message"); %>

    <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
    <% int cnt = 0; %>
    <% while (myNextFeedItem != FeedItem.None) { %>
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
            <% } %>
            <% cnt++; %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
    <% } %>
</asp:Content>
