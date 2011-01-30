<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Helpers.UserInformation.UserSettings>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<div class="col-18 m-btm10">
	<div class="round-3 p-a10 c-white fnt-14">
		<img src="/Content/images/settings.png" alt="Settings" style="vertical-align: middle;" />
		<span class="padding-10h">Settings</span>
	</div>
	<ul class="push-1 tabs fnt-12">
        <% if(Model.Equals(UserSettings.AccountSettings)) { %>
		    <li><%= Html.ActionLink("Account Settings", "Edit", "User", null, new { @class = "active" }) %></li>
		    <li><%= Html.ActionLink("Privacy Settings", "Edit", "UserPrivacySettings", null, null)%></li>
        <% } %>
        <% if(Model.Equals(UserSettings.PrivacySettings)) { %>
		    <li><%= Html.ActionLink("Account Settings", "Edit", "User", null, null) %></li>
		    <li><%= Html.ActionLink("Privacy Settings", "Edit", "UserPrivacySettings", null, new { @class = "active" }) %></li>
        <% } %>
	</ul>
	<div class="clear">&nbsp;</div>
</div>
<div class="clear">&nbsp;</div>