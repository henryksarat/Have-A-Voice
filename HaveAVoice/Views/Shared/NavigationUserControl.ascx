<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<% User myUser = HAVUserInformationFactory.GetUserInformation().Details; %>
<div class="col-12">
    <ul>
        <li><a href="#">HOME</a></li>
		<li><a href="#">FANS<span class="alert"><%= NavigationHelper.PendingFanCount(myUser) %></span></a></li>
		<li><a href="#">MAIL<span class="alert"><%= NavigationHelper.NewMessageCount(myUser) %></span></a></li>
		<li><a href="#">NOTIFICATIONS<span class="alert"><%= NavigationHelper.NotificationCount(myUser) %></span></a></li>
    </ul>
</div>
<div class="col-6">
	<ul class="right">
        <li><a href="#">SETTINGS</a></li>
        <li><%= Html.ActionLink("LOGOUT", "LogOut", "Authentication")%></li>
    </ul>
</div>