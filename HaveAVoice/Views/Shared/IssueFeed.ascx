<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.IssueFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<% int myCount = (int)ViewData["Count"]; %>
<div class="<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm10">
	<div class="col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.Username %>" class="profile" />
	</div>
	<div class="col-16">
		<div class="m-lft col-16 comment">
			<span class="speak-lft">&nbsp;</span>
			<div class="p-a10">
				<h1><a href="/Issue/View/<%= Model.Id %>"><%= Model.Title%></a></h1>
				<br />
				<%= Model.Description %>

				<div class="clear">&nbsp;</div>

				<div class="col-15">
					<div class="push-6 col-9 p-v10">
						<div class="col-3 center">
							<% if (Model.TotalReplys == 0) { %>
								&nbsp;
								<!--
								<a href="#" class="comment">
										Reply
								</a>
								//-->
							<% } else { %>
								<span class="comment"><%= Model.TotalReplys%> 
										Repl<% if (Model.TotalReplys > 1) { %>ies<% } else { %>y<% } %>
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
								<a href="<%= LinkHelper.LikeIssue(Model.Id) %>" class="like">Like</a>
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
								<a href="<%= LinkHelper.DislikeIssue(Model.Id) %>" class="dislike">Dislike</a>
							<% } %>
						</div>
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
<div class="clear">&nbsp;</div>
			    
<% int j = 0; %>
<% foreach (var item in Model.IssueReplys) { %>
    <div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-2 col-19 m-btm10">
	    <div class="col-2 center">
	        <img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= item.User.Username %>" class="profile" />
	        &nbsp;
	    </div>
	    <div class="m-lft col-14 comment">
	        <span class="speak-lft">&nbsp;</span>
	        <div class="p-a10">
	            <a href="/Profile/Show/<%= item.User.Id %>" class="name"><%= item.User.Username %></a>
	            <%= item.Reply %>
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

<div class="board-reply m-btm10">
	<div class="push-2 col-19">						
		<div class="col-2 center">
            <% UserInformationModel myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
            <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
            <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			<img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile" />
		</div>
		<div class="m-lft col-14">
			<% using (Html.BeginForm("Create", "IssueReply", new { issueId = Model.Id, disposition = 1, anonymous = false })) { %>
				<%= Html.ValidationMessage("Reply", "*")%>
				<%= Html.TextArea("Reply")%>
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