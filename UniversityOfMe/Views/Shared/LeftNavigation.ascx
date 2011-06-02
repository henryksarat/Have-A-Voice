<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UniversityOfMe.Models.View.LeftNavigation>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<script>
    $(document).ready(function () {
        $("#datingtooltip a[title]").tooltip();
    });
</script> 

<div class="four"> 
	<div class="banner full mb72"> 
		<input type="search" name="search" id="search" /> 
		<span class="corner"></span> 
	</div> 
 
	<div class="banner title"> 
		NEW NOTIFICATIONS
	</div> 
	<div class="box"> 
		<ul class="notification"> 
            <% foreach (NotificationModel myNotificationModel in Model.Notifications) { %>
			    <% if (myNotificationModel.SendItem == SendItemOptions.BEER) { %>
                    <li class="beer"> 
				        <a href="/<%= myNotificationModel.WhoSent.ShortUrl %>"><%= NameHelper.FullName(myNotificationModel.WhoSent) %></a> sent you a beer
				        <span class="time"><%= myNotificationModel.FormattedDateTimeSent %></span> 
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
                <% } %>
            <% } %>
		</ul> 
	</div> 
				
    <% if(Model.HasDatingMatch()) { %>	
	    <div class="match"> 
            <%= Html.ActionLink("Close", "MarkAsSeen", "Dating", new { datingLogId = Model.DatingMatchMember.Id }, new { @class = "close" })%>
            <a href="/<%= Model.DatingMatchMember.AskedUser.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(Model.DatingMatchMember.AskedUser) %>" alt="<%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %>" title="<%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %>" class="profile med" /></a>
		    You got a dating match with<br /> 
		    <span class="normal mr14"><%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %></span> 
            
            <%= Html.ActionLink("Message", "Create", "Message", new { id = Model.DatingMatchMember.AskedUserId }, new { @class = "mail mr9" })%>
            <%= Html.ActionLink("Send beer", "SendItem", "SendItems", new { id = Model.DatingMatchMember.AskedUserId, sendItem = SendItemOptions.BEER }, new { @class = "beer" })%>
		    <span class="arrow"></span> 
	    </div> 
    <% } %>

    <% if(FeatureHelper.IsFeatureEnabled(Model.User, Features.DatingAsked)) { %> 
	    <div class="banner title"> 
		    UOFME RANDOM DATING
		    <div id="datingtooltip" class="buttons"> 
			        <a href="#" class="question" title="A dating match is made only if the person says yes too, if they say no they never know.">What is this?</a> 
			    <%= Html.ActionLink("Disable", "DisableFeature", "Profile", new { feature = Features.DatingAsked }, new { @class = "deny" })%>
		    </div> 
	    </div> 

	    <div class="box date"> 
            <% if (Model.HasDatingMember()) { %>	
                <a href="/<%= Model.DatingMember.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(Model.DatingMember) %>" alt="<%= NameHelper.FullName(Model.DatingMember) %>" title="<%= NameHelper.FullName(Model.DatingMember) %>" class="profile sm" /></a>
		        Would you date <%= NameHelper.FullName(Model.DatingMember)%> ?
		        <div class="mt6 center"> 
                    <% using (Html.BeginForm("Create", "Dating", FormMethod.Post)) {%>
                        <%= Html.Hidden("SourceUserId", Model.DatingMember.Id) %>
			            <button name="response" class="btn blue" value="true">Yes</input>
			            <button name="response" class="btn blue" value="false">No</input>
                    <% } %>
		        </div> 
            <% } else { %>
                There are currently no members to ask you about
            <% } %>
	    </div> 
    <% } %>
					
	<div class="banner title"> 
		NEWEST MEMBERS
	</div> 
	<div class="box member"> 
    <% foreach (User myNewestMember in Model.NewestUsersInUniversity) { %>
        <a href="/<%= myNewestMember.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myNewestMember) %>" title="<%= NameHelper.FullName(myNewestMember) %>" /></a>
    <% } %>
	</div> 
</div> 