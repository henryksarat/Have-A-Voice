<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.BaseWebsite.Models.ILoggedInListModel<Social.Generic.Models.InboxMessage>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Inbox</h2>
    <% using (Html.BeginForm("Inbox", "Message")) { %>

        <% foreach (var item in Model.Get()) { %>
            <%= MessageHelper.MessageList(item.FromUserId, item.FromUser, item.FromUserProfilePictureUrl, item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>    
        <% } %>

		<input type="submit" value="Delete" />
    <%} %>
</asp:Content>

