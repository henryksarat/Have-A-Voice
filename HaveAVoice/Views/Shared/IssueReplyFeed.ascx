<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.IssueReplyFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<% int myCount = (int)ViewData["Count"]; %>
<div class="<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm5">
	<div class="col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.DisplayName %>" class="profile" />
	</div>
	<div class="col-16">
		<div class="m-lft col-16 comment">
			<span class="speak-lft">&nbsp;</span>
			<div class="p-a10">
				<%= Html.ActionLink(Model.DisplayName, "Show", "Profile", new { id = Model.UserId }, new { @class = "name" })%>
				<br />
				<%= Model.Reply%>

				<div class="options p-v10">
					<div class="push-6 col-9">
						<div class="col-3 center">
							<% if (Model.TotalComments == 0) { %>
								&nbsp;
								<!--
								<a href="#" class="comment">
									Comment
								</a>
								//-->
							<% } else { %>
								<span class="comment"><%= Model.TotalComments%> 
									Comment<% if (Model.TotalComments > 1) { %>s<% } %>
								</span>
							<% } %>
						</div>
						<div class="col-3 center">
							<% if (Model.HasDisposition) { %>
								<span class="like">
									<%= Model.TotalLikes%>
									<% if (Model.TotalLikes == 1) { %>
										Person Likes
									<% } else { %>
										People Like
									<% } %>
									This
								</span>
							<% } else { %>
								<a href="#" class="like">Like</a>
							<% } %>
						</div>
						<div class="col-3 center">
							<% if (Model.HasDisposition) { %>
								<span class="dislike">
									<%= Model.TotalDislikes%>
									<% if (Model.TotalDislikes == 1) { %>
										Person Dislikes
									<% } else { %>
										People Dislike 
									<% } %>
									This
								</span>
							<% } else { %>
								<a href="#" class="dislike">Dislike</a>
							<% } %>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
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

<% int j = 0; %>
<% foreach (var item in Model.IssueReplyComments) { %>
    <div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-3 col-19 m-btm5">
	    <div class="col-1 center">
	        <img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= DisplayNameHelper.Display(item.User) %>" class="profile sm" />
	    </div>
	    <div class="m-lft col-14 comment">
	        <span class="speak-lft">&nbsp;</span>
	        <div class="p-a10">
	            <a href="#" class="name"><%= DisplayNameHelper.Display(item.User)%></a>
	            <%= item.Comment %>
	        </div>
	    </div>
	    <div class="col-3">
	        <div class="p-a5">
	            <div class="date-tile">
	                <span><%= item.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= item.DateTimeStamp.ToString("dd") %>
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
		<div class="m-lft col-12 m-rgt">
			<% using (Html.BeginForm("Create", "IssueReplyComment", new { issueReplyId = Model.Id })) { %>
				<%= Html.ValidationMessage("Comment", "*") %>
				<%= Html.TextArea("Comment") %>
			<% } %>
		</div>
		<div class="col-2 center">
			<input type="submit" value="Post" />
		</div>
		<div class="clear">&nbsp;</div>
	</div>
	<div class="clear">&nbsp;</div>
</div>