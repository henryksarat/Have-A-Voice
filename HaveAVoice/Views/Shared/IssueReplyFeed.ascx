<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.IssueReplyFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<% int myCount = (int)ViewData["Count"]; %>
<% SiteSection mySection = (SiteSection)ViewData["SiteSection"]; %>
<% int mySourceId = (int)ViewData["SourceId"]; %>
<div class="<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm5">
	<div class="push-2 col-2 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.DisplayName %>" class="profile" />
	</div>
	<div class="push-2 col-14">
		<div class="m-lft col-14 comment">
			<span class="speak-lft">&nbsp;</span>
			<div class="p-a10">
				<span class="fnt-14 teal">Regarding <a href="/Issue/Details/<%= Model.Issue.Title %>" class="issue"><%= Model.Issue.Title %></a></span><br />
				<a href="/Profile/Show/<%= Model.UserId %>" class="name"><%= Model.DisplayName %></a> says:
				<%= Model.Reply%>
				<br />
				<span class="loc"><%= Model.Issue.City %>, <%= Model.Issue.State %></span> <span class="<%= Model.IconType %>" title="<%= Model.IconType %>">&nbsp;</span>
				<div class="clear">&nbsp;</div>
				<div class="options p-v10">
					<div class="push-3 col-10">
						<div class="col-1 center">
							<a href="<%= ComplaintHelper.IssueReplyLink(Model.Id) %>" class="issue-report" title="Report">
								Report
							</a>
							<div class="clear">&nbsp;</div>
						</div>
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
										Person Agrees
									<% } else { %>
										People Agree
									<% } %>
								</span>
							<% } else { %>
								<a href="<%= LinkHelper.AgreeIssueReply(Model.Id, Model.Issue.Id, mySection, mySourceId) %>" class="like">Agree</a>
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
								<a href="<%= LinkHelper.DisagreeIssueReply(Model.Id, Model.Issue.Id, mySection, mySourceId) %>" class="dislike">Disagree</a>
							<% } %>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
			</div>
		</div>
	</div>
	<div class="push-2 col-3">
		<div class="p-a5">
			<div class="date-tile">
				<span><%= Model.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= Model.DateTimeStamp.ToString("dd")%>
			</div>
		</div>
	</div>
	<div class="clear">&nbsp;</div>
</div>

<div class="reply-wrpr">
	<% int j = 0; %>
	<% foreach (var item in Model.IssueReplyComments) { %>
	    <div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-3 col-19 m-btm5">
		    <div class="col-1 center">
                <% UserInformationModel myUserInformation = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation(); %>
                <% bool myIsAllowedToView = PrivacyHelper.IsAllowed(item.User, HaveAVoice.Helpers.Enums.PrivacyAction.DisplayProfile, myUserInformation); %>
                <% if (myIsAllowedToView) { %>
		            <img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= NameHelper.FullName(item.User) %>" class="profile sm" />
		        <% } else { %>
                    <img src="<%= HAVConstants.ANONYMOUS_PICTURE_URL %>" alt="Anonymous" class="profile sm" />
                <% } %>
                <div class="clear">&nbsp;</div>
		    </div>
		    <div class="m-lft col-14 comment">
		        <span class="speak-lft">&nbsp;</span>
		        <div class="p-a10">
                    <% if (myIsAllowedToView) { %>
		                <a href="<%= LinkHelper.Profile(item.User) %>" class="name"><%= NameHelper.FullName(item.User)%></a>
		                <%= item.Comment%>
		                <br />
		                <span class="loc"><%= item.IssueReply.Issue.City%>, <%= item.IssueReply.Issue.State%></span>
                    <% } else { %>
		                <a href"/Profile/Show" class="name">Anonymous</a>
		                <%= item.Comment%>
		                <br />
                    <% } %>
		            <div class="clear">&nbsp;</div>
		        </div>
		        <div class="clear">&nbsp;</div>
		    </div>
		    <div class="col-3">
		        <div class="p-a5">
		            <div class="date-tile">
		                <span><%= item.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= item.DateTimeStamp.ToString("dd") %>
		            </div>
		        </div>
		        <div class="clear">&nbsp;</div>
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
			<div class="clear">&nbsp;</div>
		</div>
		<% using (Html.BeginForm("Create", "IssueReplyComment", new { issueReplyId = Model.Id })) { %>
		    <div class="m-lft col-12 m-rgt">
			    <%= Html.TextArea("Comment") %>
				<span class="req">
					<%= Html.ValidationMessage("Comment", "*") %>
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