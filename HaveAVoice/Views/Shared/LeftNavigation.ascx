<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<% bool myIsMyProfile = Model.SiteSection == SiteSection.MyProfile; %>
<% bool myIsAllowedToView = PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile); %>
<% if (!myIsMyProfile && myIsAllowedToView) { %>
	<div class="col-3 profile">
		<div class="col-3 p-v10 details">
			<span class="blue">Name:</span> <br /><%= Model.FullName %><br />
			<span class="blue">Gender:</span> <br /><%= Model.User.Gender %><br />
			<span class="blue">Birthday:</span> <br /><%= Model.User.DateOfBirth.ToShortDateString() %><br />
		    <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
<% } else { %>
	&nbsp;
<% } %>