<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (HAVUserInformationFactory.IsLoggedIn()) { %>
    <ul class="main-tabs font-12">
        <li><%=Html.ActionLink("Popular", "Tabs") %></li>
        <li><%=Html.ActionLink("Network", "Tabs") %></li>
    </ul>
<% } else { %>
    <ul class="main-tabs font-12">
		<li><a class="active" href="javascript:void(0);">Latest Ideas</a></li>
	</ul>
<% } %>
<div class="clear">&nbsp;</div>