<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.BoardFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<% int myCount = (int)ViewData["Count"]; %>
<div class="boards">
<div class="<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm10">
	<div class="col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.DisplayName %>" class="profile" />
	</div>
	<div class="col-16 m-btm10">
		<div class="m-lft col-16 comment">
			<span class="speak-lft">&nbsp;</span>
			<div class="p-a10">
				<a class="name" href="#"><%= Model.DisplayName%></a>
                    <%= Model.Message%>
				<div class="clear">&nbsp;</div>

				<div class="spacer-10">&nbsp;</div>
							
				<div class="clear">&nbsp;</div>
				<div class="options">
					<div class="col-6">&nbsp;</div>
					<div class="col-9">
						<div class="col-3 center">
							<!--
							<a href="#" class="comment">COMMENT</a>
							//-->
						</div>
						<div class="col-3 center">
							<a href="#" class="like">LIKE</a>
						</div>
						<div class="col-3 center">
							<a href="#" class="dislike">DISLIKE</a>
						</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
				<div class="spacer-10">&nbsp;</div>
				<div class="clear">&nbsp;</div>
			</div>
		</div>
	</div>
	<div class="col-3">
		<div class="p-a5">
			<div class="date-tile">
				<span><%= Model.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= Model.DateTimeStamp.ToString("dd")%>
			</div>
		</div>
	</div>
	<div class="clear">&nbsp;</div>
</div>

<div class="board-comments">
<% int j = 0; %>
<% foreach (BoardReply myReply in Model.BoardReplys) { %>		
    <div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-2 col-19 m-btm10">
	    <div class="col-2 center">
	        <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" alt="<%= myReply.User.Username %>" class="profile" />
	        &nbsp;
	    </div>
	    <div class="m-lft col-14 comment">
	        <span class="speak-lft">&nbsp;</span>
	        <div class="p-a10">
	            <a href="#" class="name"><%= myReply.User.Username %></a>
	            <%= myReply.Message %>
	        </div>
	    </div>
	    <div class="col-3">
	        <div class="p-a5">
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

<div class="board-reply m-btm10">
	<div class="push-2 col-19">
		<div class="col-2 center">
            <% UserInformationModel myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
            <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
            <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			<img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile" />
		</div>
		<div class="m-lft col-14">
			<% using (Html.BeginForm("Create", "BoardReply", new { sourceUserId = Model.UserId, boardId = Model.Id })) { %>
		        <%= Html.ValidationMessage("Message", "*")%>
		        <%= Html.TextArea("Message")%>
		        <div class="clear">&nbsp;</div>
		        <div class="right m-top10">
		            <input type="submit" value="Post" />
		        </div>
			<% } %>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
	<div class="clear">&nbsp;</div>
</div>

</div>
</div>