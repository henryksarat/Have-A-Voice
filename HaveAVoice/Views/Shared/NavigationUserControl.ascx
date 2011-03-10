﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<% User myUser = HAVUserInformationFactory.GetUserInformation().Details; %>
<div class="col-12 fnt-12">
    <ul>
        <li><a href="/Profile/Show">HOME</a></li>
		<li>
			FRIENDS<%= NavigationCountHelper.PendingFriendCount(myUser)%>
			<ul>
				<li><a href="/Friend/List">Friends</a></li>
				<li><a href="/Friend/Pending">FRIEND REQUESTS</a></li>
			</ul>
		</li>
		<li><a href="/Message/Inbox">MAIL<%= NavigationCountHelper.NewMessageCount(myUser)%></a></li>
        <li><a href="/Notification/List">NOTIFICATIONS<%= NavigationCountHelper.NotificationCount(myUser)%></a></li>
		<li><a href="/Issue/Index">ISSUES</a></li>
    </ul>
</div>
<div class="col-6 fnt-12">
	<ul class="right">
        <li><a href="/User/Edit">SETTINGS</a></li>
        <li><%= Html.ActionLink("LOGOUT", "LogOut", "Authentication")%></li>
    </ul>
</div>