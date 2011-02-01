<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
	&nbsp;
<% } else { %>
	&nbsp;
<% } %>