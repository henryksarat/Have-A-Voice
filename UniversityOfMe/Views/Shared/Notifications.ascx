<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<NotificationModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="System.Collections.Generic" %>

<ul class="notification"> 
    <% foreach (NotificationModel myNotificationModel in Model) { %>
        <% if(myNotificationModel.NotificationType == NotificationType.SentItems) %>
			<% if (myNotificationModel.SendItem == SendItemOptions.BEER) { %>
                <li class="beer"> 
				    <a href="/<%= myNotificationModel.WhoSent.ShortUrl %>"><%= NameHelper.FullName(myNotificationModel.WhoSent) %></a> sent you a beer
				    <span class="time"><%= DateHelper.ToLocalTime(myNotificationModel.DateTimeSent) %></span> 
			    </li> 
            <% } else { %>
			    <li class="friend"> 
				    <a href="#">Anca Foster</a> sent you a friend request
			    </li> 
			    <li class="post"> 
				    <a href="#">Anca Foster</a> replied to <a href="#">your post</a> 
				    <span class="time">6:35 pm</span> 
			    </li> 
			    <li class="post"> 
				    <a href="#">Anca Foster</a> replied to <a href="#">your post</a> 
				    <span class="time">6:35 pm</span> 
			    </li> 
        <% } else if(myNotificationModel.NotificationType == NotificationType.Club) { %>
			<li class="organization"> 
				<a href="<%= URLHelper.ProfileUrl(myNotificationModel.ClubMemberUser) %>"><%= NameHelper.FullName(myNotificationModel.ClubMemberUser) %></a> wants to join <a href="<%= URLHelper.BuildClubUrl(myNotificationModel.Club) %>"><%= myNotificationModel.Club.Name %> </a> 
				<span class="time">6:35 pm</span> 
			</li> 
        <% } %>
    <% } %>
</ul> 