<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Helpers.UserInformation.UserSettings>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<div class="grid_13">
	<div class="round-3 padding-10a white font-14">
		<img src="/Content/images/settings.png" alt="Settings" style="vertical-align: middle;" />
		<span class="padding-10h">Settings</span>
	</div>
	<ul class="push_1 tabs font-12">
        <% if(Model.Equals(UserSettings.AccountSettings)) { %>
		    <li><%= Html.ActionLink("Account Settings", "Edit", "User", null, new { @class = "active" })%></li>
		    <li><%= Html.ActionLink("Privacy Settings", "Edit", "UserPrivacySettings", null, null)%></li>
        <% } %>
        <% if(Model.Equals(UserSettings.PrivacySettings)) { %>
		    <li><%= Html.ActionLink("Account Settings", "Edit", "User", null, null)%></li>
		    <li><%= Html.ActionLink("Privacy Settings", "Edit", "UserPrivacySettings", null, new { @class = "active" })%></li>
        <% } %>
	</ul> 
</div>
<div class="clear">&nbsp;</div>