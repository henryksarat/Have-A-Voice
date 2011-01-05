<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<InboxMessage>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.UserModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
	    <% using (Html.BeginForm("Inbox", "Message", FormMethod.Post, new { @class = "create"})) { %>
			<%= Html.Encode(ViewData["Message"]) %>
			<%= Html.Encode(TempData["Message"]) %>
			<div class="clear">&nbsp;</div>

			<div class="action-bar p-a10 m-btm20 right">
				<input type="submit" value="Delete" class="btn" />
			</div>

			<% if (Model != null) { %>
                <% foreach (var item in Model.Models) { %>
                    <%= MessageHelper.MessageList(item.FromUserId, item.FromUsername, item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>
                <% } %>
            <% } %>
        <% } %>
	</div>
</asp:Content>

