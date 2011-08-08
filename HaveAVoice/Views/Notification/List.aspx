<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<NotificationModel>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

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
        <div class="action-bar bold p-a10 m-btm20 color-4">
        	Notifications
        </div>

        <% Html.RenderPartial("Message"); %>
        <div class="clear">&nbsp;</div>

	    <% foreach (var item in Model.Models) { %>
	    	<div class="notification m-btm10 p-v10 fnt-12">
                <% if (item.NotificationType == NotificationType.Issue) { %>
	    		    New replies in your issue: <%= Html.ActionLink(item.Label, "Details", "Issue", new { id = item.Id }, null)%> <div class="f-rgt">
                <% } else if (item.NotificationType == NotificationType.ParticipatingBoard) { %>
                    New comments to a board message you are participating in: <%= Html.ActionLink(item.Label, "Details", "Board", new { id = item.Id }, null)%> <div class="f-rgt">
                <% } else if (item.NotificationType == NotificationType.Board) { %>
                    <a href="<%= LinkHelper.Profile(item.TriggeredUser) %>">
                        <%= NameHelper.FullName(item.TriggeredUser) %>
                    </a> 
                    posted a new 
                    <%= Html.ActionLink("message", "Details", "Board", new { id = item.Id }, null)%> 
                    on 
                    <a href="<%= LinkHelper.Profile(Model.NavigationModel.User) %>">
                        your board
                    </a>.
                <% } else if (item.NotificationType == NotificationType.ParticipatingIssueReply) { %>
                    New comments to a reply to an issue you are participating in: 
                    <%= Html.ActionLink(item.Label, "Details", "IssueReply", new { id = item.Id }, null)%> 
                <% } else if(item.NotificationType == NotificationType.IssueReply) { %>
                    Someone commented on your 
                    <%= Html.ActionLink("reply", "Details", "IssueReply", new { id = item.Id }, null)%> to an issue.
                <% } else if(item.NotificationType == NotificationType.GroupBoardPost) { %>
                    Someone posted to the board of the group 
                    <%= Html.ActionLink(item.Label, "Details", "Group", new { id = item.Id }, null)%>.
                <% } else if(item.NotificationType == NotificationType.UnapprovedGroupMember) { %>
                    <%= Html.ActionLink(NameHelper.FullName(item.TriggeredUser), "Details", "GroupMember", new { groupId = item.SecondaryId, groupMemberId = item.Id }, null)%> 
                    wants to join the group 
                    <a href="<%= LinkHelper.GroupUrl(item.SecondaryId) %>"><%= item.Label %></a>.
                    <%= Html.ActionLink("Approve/Deny", "Details", "GroupMember", new { groupId = item.SecondaryId, groupMemberId = item.Id }, null)%>
                <% } else if(item.NotificationType == NotificationType.GroupInvitation) { %>
                    <a href="<%= LinkHelper.Profile(item.TriggeredUser) %>"><%= NameHelper.FullName(item.TriggeredUser) %></a> 
                    invited you to the group
                    <a href="<%= LinkHelper.GroupUrl(item.SecondaryId) %>"><%= item.Label %></a>.
                    <%= Html.ActionLink("Accept Invitation", "AcceptInvite", "GroupMember", new { groupId = item.SecondaryId, groupInvitationId = item.Id }, null)%>
                    or
                    <%= Html.ActionLink("Decline Invitation", "DeclineInvite", "GroupMember", new { groupId = item.SecondaryId, groupInvitationId = item.Id }, null)%>
                <% } %>

                <div class="f-rgt"><%= LocalDateHelper.ToLocalTime(item.DateTimeStamp) %></div>
	    		<div class="clear">&nbsp;</div>
	    	</div>
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>
