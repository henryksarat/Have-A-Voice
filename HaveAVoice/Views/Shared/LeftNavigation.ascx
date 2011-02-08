<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
	<div class="col-3 profile">
		<div class="col-3 p-v10 details">
			<span class="blue">Name:</span> <% /* = Model.User.FirstName + " " + Model.User.LastName */ %><br />
			<span class="blue">Gender:</span> <% /* = Model.User.Gender */ %><br />
			<span class="blue">Site:</span> <% /* = Model.User.Website */ %><br />
		    <span class="blue">Email:</span> <% /* = Model.User.Email */ %><br />
		    <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
<% } else { %>
	&nbsp;
<% } %>