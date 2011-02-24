<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<NotificationModel>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Notifications
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

	    <% foreach (var item in Model.Models) { %>
	    	<div class="notification m-btm20">
                <% if (item.NotificationType == NotificationType.Issue) { %>
	    		    New replies in your issue: <%= Html.ActionLink(item.Label, "Details", "Issue", new { id = item.Id }, null)%> (<%= item.DateTimeStamp %>)
                <% } else if (item.NotificationType == NotificationType.ParticipatingBoard) { %>
                    New comments to a board message you are participating in: <%= Html.ActionLink(item.Label, "Details", "Board", new { id = item.Id }, null)%> (<%= item.DateTimeStamp %>)
                <% } else if (item.NotificationType == NotificationType.Board) { %>
                    ad
                <% } %>
	    		<div class="clear">&nbsp;</div>
	    	</div>
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>
