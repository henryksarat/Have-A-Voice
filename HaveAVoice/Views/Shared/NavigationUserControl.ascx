<%@ Control Language=   "C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<% User myUser = HAVUserInformationFactory.GetUserInformation().Details; %>
<div class="col-18 fnt-12">
    <ul>
        <li><a href="/Profile/Show">HOME</a></li>
		<li>
			FRIENDS<%= NavigationCountHelper.PendingFriendCount(myUser)%>
			<ul>
				<li><a href="/Friend/List">Friends</a></li>
				<li><a href="/Friend/Pending">Friend Requests</a></li>
			</ul>
		</li>
		<li><a href="/Message/Inbox">MAIL<%= NavigationCountHelper.NewMessageCount(myUser)%></a></li>
        <li><a href="/Notification/List">NOTIFICATIONS<%= NavigationCountHelper.NotificationCount(myUser)%></a></li>
		<li>ISSUES
            <ul>
                <li>
                    <a href="/Issue/Create">Create Issue</a>
                    <a href="/Issue/Index">Find Issues</a>
                    <a href="/Petition/Create">Create Petition</a>
                    <a href="/Petition/List">Find Petition</a>
                </li>
            </ul>
        </li>
        <li>GROUPS
            <ul>
                <li>
                    <a href="/Group/Create">Create</a>
                    <a href="/Group/List">Find a Group</a>
                    <a href="/Group/MyGroups">Groups I'm In</a>
                </li>
            </ul>
        </li>
        <li><a href="/User/Edit">SETTINGS</a></li>
        <li><%= Html.ActionLink("LOGOUT", "LogOut", "Authentication")%></li>
    </ul>
</div>
