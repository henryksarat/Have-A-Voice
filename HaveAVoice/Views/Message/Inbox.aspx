<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<InboxMessage<User>>>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>
    
    <div class="col-21">
        <% Html.RenderPartial("Message"); %>
        <div class="clear">&nbsp;</div>

	    <% using (Html.BeginForm("Inbox", "Message", FormMethod.Post, new { @class = "create"})) { %>
			<div class="action-bar p-a10 m-btm20">
				<% /* Html.ActionLink("New Message", "Create", "Message", null, new { @class = "btn f-lft" }) */ %>
				<input type="submit" value="Delete" class="btn f-rgt" />
				<div class="clear">&nbsp;</div>
			</div>

			<% if (Model != null) { %>
                <% foreach (var item in Model.Models) { %>
                    <%= MessageHelper.MessageList(item.FromUser.Id, NameHelper.FullName(item.FromUser), PhotoHelper.ProfilePicture(item.FromUser), item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>
                <% } %>
            <% } %>
        <% } %>
	</div>
</asp:Content>

