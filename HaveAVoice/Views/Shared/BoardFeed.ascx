<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.BoardFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<% int myCount = (int)ViewData["Count"]; %>
<div class="board-<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 m-btm5">
	<div class="col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.DisplayName %>" class="profile" />
	</div>
	<div class="col-16 m-btm10">
		<div class="m-lft col-16">
			<div class="p-h10">
				<a class="name" href="#"><%= Model.DisplayName%></a>
                <%= Model.Message%>
				<div class="clear">&nbsp;</div>
			</div>
		</div>
	</div>
	<div class="col-3 right">
		<div class="p-h5">
			<div class="date-tile">
				<span><%= Model.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= Model.DateTimeStamp.ToString("dd")%>
			</div>
		</div>
	</div>
	<div class="clear">&nbsp;</div>
</div>

<% int j = 0; %>
<% foreach (BoardReply myReply in Model.BoardReplys) { %>		
    <div class="board-<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 push-3 col-18 m-btm5">
	    <div class="col-1 center">
	        <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" alt="<%= myReply.User.Username %>" class="profile sm" />
	    </div>
	    <div class="m-lft col-14">
	        <div class="p-h10">
	            <a href="#" class="name"><%= myReply.User.Username %></a>
	            <%= myReply.Message %>
	        </div>
	    </div>
	    <div class="col-3 right">
	        <div class="p-h5">
	            <div class="date-tile">
	                <span><%= myReply.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= myReply.DateTimeStamp.ToString("dd") %>
	            </div>
	        </div>
	    </div>
	    <div class="clear">&nbsp;</div>
	</div>
	<div class="clear">&nbsp;</div>
	<% j++; %>
<% } %>
	
<div class="board-reply m-btm5">
	<div class="push-3 col-19">
		<div class="col-1 center">
            <% UserInformationModel myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
            <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
            <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			<img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile sm" />
		</div>
		<% using (Html.BeginForm("Create", "BoardReply", new { sourceUserId = Model.UserId, boardId = Model.Id })) { %>
			<div class="m-lft col-14">
				<div class="alpha col-12">
			        <%= Html.ValidationMessage("Message", "*")%>
			        <%= Html.TextArea("Message")%>
				</div>
				<div class="col-2 center">
		            <input type="submit" value="Post" />
		        </div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		<% } %>
	</div>
	<div class="clear">&nbsp;</div>
</div>