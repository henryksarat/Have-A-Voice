<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<InboxMessage>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
        <div class="clear">&nbsp;</div>
    </div>
    
    <div class="col-21">
        <% Html.RenderPartial("Message"); %>
        <div class="clear">&nbsp;</div>

	    <% using (Html.BeginForm("Inbox", "Message", FormMethod.Post, new { @class = "create"})) { %>
			<div class="action-bar p-a10 m-btm20">
				<%= Html.ActionLink("New Message", "Create", "Message", null, new { @class = "btn f-lft" }) %>
				<input type="submit" value="Delete" class="btn f-rgt" />
				<div class="clear">&nbsp;</div>
			</div>

			<% if (Model != null) { %>
                <% foreach (var item in Model.Models) { %>
                    <%= MessageHelper.MessageList(item.FromUserId, item.FromUser, item.FromUserProfilePictureUrl, item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>
                <% } %>
            <% } %>
        <% } %>
	</div>
</asp:Content>

