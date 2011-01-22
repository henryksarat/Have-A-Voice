<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
	&nbsp;
<% } else { %>
	<ul>
		<li class="first"><a href="/Home/FriendFeed">Friend Feed</a></li>
		<li><a href="/Home/OfficialsFeed">Officials Feed</a></li>
		<li class="last"><a href="/Home/FilteredFeed">Filtered Feed</a></li>
	</ul>
<% } %>