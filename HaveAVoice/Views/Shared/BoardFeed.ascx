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
				<a class="name" href="/Profile/Show/<%= Model.UserId %>"><%= Model.DisplayName%></a>
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

<div class="board-wrpr">
	<% int j = 0; %>
	<% foreach (BoardReply myReply in Model.BoardReplys) { %>		
	    <div class="board-<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 push-3 col-18 m-btm5">
		    <div class="col-1 center">
		        <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" alt="<%= NameHelper.FullName(myReply.User) %>" class="profile sm" />
		    </div>
		    <div class="m-lft col-14">
		        <div class="p-h10">
		            <a href="<%= LinkHelper.Profile(myReply.User) %>" class="name"><%= NameHelper.FullName(myReply.User)%></a>
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
</div>
	
<div class="board-reply m-btm5">
	<a href="#" rel="reply" class="push-17 alpha col-1 omega center button">Reply</a>
	<div class="push-3 col-19" style="display:none;">
		<div class="col-1 center">
            <% UserInformationModel myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
            <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
            <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			<img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile sm" />
		</div>
		<% using (Html.BeginForm("Create", "BoardReply", new { ownerUserId = Model.OwnerUserId, boardId = Model.Id })) { %>
			<div class="m-lft col-14">
				<div class="alpha col-12">
			        <%= Html.ValidationMessage("BoardReply", "*")%>
			        <%= Html.TextArea("BoardReply")%>
				</div>
				<div class="col-2 right">
		            <input type="submit" value="Post" />
		            <div class="clear">&nbsp;</div>
		        </div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		<% } %>
	</div>
	<div class="clear">&nbsp;</div>
</div>