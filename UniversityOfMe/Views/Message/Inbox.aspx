<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Social.Generic.Models.InboxMessage>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm("Inbox", "Message")) { %>

        <% foreach (var item in Model.Get()) { %>
            <%= MessageHelper.MessageList(item.FromUserId, item.FromUser, item.FromUserProfilePictureUrl, item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>    
        <% } %>

		<input type="submit" value="Delete" />
    <%} %>
</asp:Content>

