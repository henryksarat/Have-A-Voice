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
        <% } else if(myNotificationModel.NotificationType == NotificationType.Club) { %>
			<li class="organization"> 
				<a href="<%= URLHelper.ProfileUrl(myNotificationModel.ClubMemberUser) %>"><%= NameHelper.FullName(myNotificationModel.ClubMemberUser) %></a> wants to join <a href="<%= URLHelper.BuildClubUrl(myNotificationModel.Club) %>"><%= myNotificationModel.Club.Name %> </a> 
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } else if(myNotificationModel.NotificationType == NotificationType.ClubBoard) { %>
			<li class="organization"> 
				New club board post in the <a href="<%= URLHelper.BuildClubUrl(myNotificationModel.Club) %>"><%= myNotificationModel.Club.Name %> </a> .
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } else if(myNotificationModel.NotificationType == NotificationType.ClassBoard) { %>
			<li class="class"> 
				New post in your enrolled class <a href="<%= URLHelper.BuildClassDiscussionUrl(myNotificationModel.Class) %>"><%= myNotificationModel.Class.ClassCode%> </a> 
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } else if (myNotificationModel.NotificationType == NotificationType.None) { %>
			<li class="minilogo"> 
                You have no notifications.
			</li> 
        <% } else if (myNotificationModel.NotificationType == NotificationType.ClassBoardReply) { %>
			<li class="class"> 
                <% if (myNotificationModel.IsMine) { %>
                    Reply to your class discussion post in <a href="<%= URLHelper.BuildClassBoardUrl(myNotificationModel.ClassBoard) %>"><%= myNotificationModel.ClassBoard.Class.ClassCode%></a>.
                <% } else { %>
                    Reply to a class discussion in <a href="<%= URLHelper.BuildClassBoardUrl(myNotificationModel.ClassBoard) %>"><%= myNotificationModel.ClassBoard.Class.ClassCode%></a>.
                <% } %>
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span>
			</li>             
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
        <% } else if(myNotificationModel.NotificationType == NotificationType.Event) { %>
			<li class="event"> 
				New post in the event <a href="<%= URLHelper.EventUrl(myNotificationModel.Event) %>"><%= TextShortener.Shorten(myNotificationModel.Event.Title, 20) %> </a> 
				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } else if (myNotificationModel.NotificationType == NotificationType.GeneralPosting) { %>
			<li class="posting"> 
                New reply in the general posting <a href="<%= URLHelper.BuildGeneralPostingsUrl(myNotificationModel.GeneralPosting) %>"><%= TextShortener.Shorten(myNotificationModel.GeneralPosting.Title, 20) %></a>.

				<span class="time"><%= LocalDateHelper.ToLocalTime(myNotificationModel.DateTimeSent)%></span> 
			</li> 
        <% } %>
    <% } %>
</ul> 