<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.IssueFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="Social.Generic.Models" %>

<% int myCount = (int)ViewData["Count"]; %>
<% SiteSection mySection = (SiteSection)ViewData["SiteSection"]; %>
<% int mySourceId = (int)ViewData["SourceId"]; %>
<div class="m-btm5">
	<div class="col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.DisplayName %>" class="profile" />
	</div>
	<div class="col-16">
		<div class="m-lft col-16 comment">
			<div class="p-a10">
				<h1><a href="<%= LinkHelper.IssueUrl(Model.Title) %>"><%= Model.Title%></a></h1>
				<br />
				<%= Model.Description %>
				<br />
				<span class="loc"><%= Model.City %>, <%= Model.State %></span> <span class="<%= Model.IconType %>" title="<%= Model.IconType %>">&nbsp;</span>
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
							<div class="clear">&nbsp;</div>
						</div>
						<div class="col-3 center">
							<% if (Model.HasDisposition) { %>
								<span class="like">
									<%= Model.TotalLikes%>
									<% if (Model.TotalLikes == 1) { %>
										Person Agrees
									<% } else { %>
										People Agree
									<% } %>
								</span>
							<% } else { %>
								<a href="<%= LinkHelper.AgreeIssue(Model.Id, mySection, mySourceId) %>" class="like">Agree</a>
							<% } %>
						</div>
						<div class="col-3 center">
							<% if (Model.HasDisposition) { %>
								<span class="dislike">
									<%= Model.TotalDislikes%>
									<% if (Model.TotalDislikes == 1) { %>
										Person Disagrees
									<% } else { %>
										People Disagree 
									<% } %>
								</span>
							<% } else { %>
								<a href="<%= LinkHelper.DisagreeIssue(Model.Id, mySection, mySourceId) %>" class="dislike">Disagree</a>
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


<div class="reply-wrpr">
	<% int j = 0; %>
	<% foreach (var item in Model.IssueReplys) { %>
	    <div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-3 col-18 m-btm10">
		    <div class="col-1 center">
                <% UserInformationModel<User> myUserInformation = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation(); %>
                <% bool myIsAllowedToView = true; %>
                <% if (item.Anonymous || !PrivacyHelper.IsAllowed(item.User, HaveAVoice.Helpers.Enums.PrivacyAction.DisplayProfile, myUserInformation)) { %>
                    <% myIsAllowedToView = false; %>
                    <img src="<%= HAVConstants.ANONYMOUS_PICTURE_URL %>" alt="Anonymous" class="profile sm" />
                <% } else { %>
		            <img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= NameHelper.FullName(item.User) %>" class="profile sm" />
                    <% } %>
		    </div>
		    <div class="m-lft col-14 comment">
		        <span class="speak-lft">&nbsp;</span>
		        <div class="p-a10">
                    <% if (item.Anonymous || !myIsAllowedToView) { %>
                        <a href="/Profile/Show" class="name">Anonymous</a>
                    <% } else { %>
		                <a href="/Profile/Show/<%= item.User.Id %>" class="name"><%= NameHelper.FullName(item.User)%></a>
                    <% } %>
		            <%= item.Reply %>
		            <br />
		            <span class="loc"><%= item.Issue.City %>, <%= item.Issue.State %></span>
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
</div>

<% UserInformationModel<User> myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
<% if(myUserModel != null) { %>
    <div class="board-reply m-btm5">
	    <a href="#" rel="reply" class="push-17 alpha col-1 omega center button">Reply</a>
	    <div class="push-3 col-19" style="display:none;">
		    <div class="col-1 center">
                <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
                <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			    <img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile sm" />
			    <div class="clear">&nbsp;</div>
		    </div>

		    <% using (Html.BeginForm("Create", "IssueReply", new { issueId = Model.Id, disposition = 1, anonymous = false })) { %>
		    <div class="m-lft col-12 m-rgt">
			    <%= Html.TextArea("Reply") %>
			    <span class="req">
				    <%= Html.ValidationMessage("Reply", "*") %>
			    </span>
			    <div class="clear">&nbsp;</div>
		    </div>
		    <div class="col-2 right">
			    <input type="submit" value="Post" />
			    <div class="clear">&nbsp;</div>
		    </div>
		    <% } %>

		    <div class="clear">&nbsp;</div>
	    </div>
	    <div class="clear">&nbsp;</div>
    </div>
<% } %>