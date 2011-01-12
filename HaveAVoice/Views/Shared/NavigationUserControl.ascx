<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<% User myUser = HAVUserInformationFactory.GetUserInformation().Details; %>
<div class="col-12">
    <ul>
        <li><a href="/Profile/Show">HOME</a></li>
		<li><a href="/Friend/List">FRIENDS<%= NavigationHelper.PendingFriendCount(myUser) %></a></li>
		<li><a href="/Message/Inbox">MAIL<%= NavigationHelper.NewMessageCount(myUser) %></a></li>
		<li><a href="/Notification/List">NOTIFICATIONS<%= NavigationHelper.NotificationCount(myUser) %></a></li>
    </ul>
</div>
<div class="col-6">
	<ul class="right">
        <li><a href="/User/Edit">SETTINGS</a></li>
        <li><%= Html.ActionLink("LOGOUT", "LogOut", "Authentication")%></li>
    </ul>
</div>