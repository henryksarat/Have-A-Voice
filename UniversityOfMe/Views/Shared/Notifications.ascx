<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<NotificationModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="System.Collections.Generic" %>

<ul class="notification"> 
    <% foreach (NotificationModel myNotificationModel in Model) { %>
        <% if(myNotificationModel.NotificationType == NotificationType.SentItems) { %>
			<% if (myNotificationModel.SendItem == SendItemOptions.BEER) { %>
                <li class="beer"> 
				    <a href="/<%= myNotificationModel.WhoSent.ShortUrl %>">
                        <%= NameHelper.FullName(myNotificationModel.WhoSent) %></a> sent you a beer 
                        <a class="disable-feature" href="<%= URLHelper.MarkSentItemAsSeen(myNotificationModel.Id) %>">
                            Hide
                        </a>
				    <span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			    </li> 
            <% } %>
        <% } else if(myNotificationModel.NotificationType == NotificationType.ClassBoard) { %>

        <% } else if (myNotificationModel.NotificationType == NotificationType.None) { %>
			<li class="minilogo"> 
                You have no notifications.
			</li> 
        <% } else if (myNotificationModel.NotificationType == NotificationType.ClassBoardReply) { %>

        <% } else if (myNotificationModel.NotificationType == NotificationType.Board) { %>
			<li class="board"> 
                <% if (myNotificationModel.IsMine) { %>
				    <a href="<%= URLHelper.ProfileUrl(myNotificationModel.Board.PostedByUser) %>">
                        <%= NameHelper.FullName(myNotificationModel.Board.PostedByUser) %>
                    </a>
                    posted a new <a href="<%= URLHelper.BoardDetailsUrl(myNotificationModel.Board) %>">board message</a>.
                <% } else { %>
                    New post to the board <a href="<%= URLHelper.BoardDetailsUrl(myNotificationModel.Board) %>"><%= TextShortener.Shorten(myNotificationModel.Board.Message, 20)%></a>
                <% } %>
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } %>
    <% } %>
</ul> 