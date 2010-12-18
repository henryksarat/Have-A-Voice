<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
	&nbsp;
<% } else { %>
	<ul>
		<li class="first"><a href="#">Menu 1</a></li>
		<li><a href="#">Menu 2</a></li>
		<li><a href="#">Menu 3</a></li>
		<li class="last"><a href="#">Menu 4</a></li>
	</ul>
<% } %>