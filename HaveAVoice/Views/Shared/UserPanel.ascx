<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<div class="col-24 user-panel">
    <% UserInformationModel myUser = HAVUserInformationFactory.GetUserInformation(); %>
	<div class="col-3 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.FullName %>" class="profile" />
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
          
            <%= NavigationHelper.UserNavigation(Model.SiteSection, Model.UserMenuMetaData, Model.User) %>
            
            <% if(myUser != null) { %>
                <% if (Model.SiteSection == SiteSection.Profile && (myUser.Details.Id != Model.User.Id)) { %>
                    <% if (!FanHelper.IsFan(Model.User.Id, myUser.Details)) { %>
                        <div class="f-rgt">
            	            <div class="col-2 center">
	            	            <a href="/Fan/Add/<%= Model.User.Id %>" class="filter like">
			                        Fan
		                        </a>
		                        <div class="clear">&nbsp;</div>
	                        </div>
			            </div>
                    <% } else {%>
                        <div class="f-rgt">
            	            <div class="col-2 center">
	            	            <a href="/Fan/Remove/<%= Model.User.Id %>" class="filter like">
			                        De-Fan
		                        </a>
		                        <div class="clear">&nbsp;</div>
	                        </div>
			            </div>
                    <% } %>
                <% } %>
            <% } %>
            <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>

		<div class="m-lft col-15 m-rgt user-control">
			<h1>
				<%= Model.User.FirstName + " " + Model.User.LastName %>
			</h1>
			<% bool myIsAuthority = RoleHelper.IsPolitician(Model.User) || RoleHelper.IsPoliticalCandidate(Model.User); %>
            <% if (myUser != null) { %>
                <% if (Model.SiteSection == SiteSection.Profile) { %>
                    <% if (!FriendHelper.IsFriend(Model.User, myUser.Details) && !myIsAuthority) { %>
            	        <div class="col-6 m-top10">
            	    	    <% using (Html.BeginForm("Add", "Friend", new { id = Model.User.Id })) { %>
				                <input type="submit" class="fan" value="Become a Friend" />
			                <% } %>
			                <div class="clear">&nbsp;</div>
		                </div>
		                <div class="clear">&nbsp:</div>
                    <% } %>
                    <% if (myUser.Details.Id != Model.User.Id) { %>
			            <a class="p-v5 m-top10 msg" href="/Message/Create/<%= Model.User.Id %>">Send <%= Model.User.FirstName%> a private message</a>
			        <% } %>
                <% } %>
            <% } %>
            <div class="clear">&nbsp;</div>
        	<% if(Model.SiteSection == SiteSection.MyProfile) { %>
				<div class="m-btm5 m-top20 m-lft col-14 m-rgt local">
                    <% if (Model.LocalIssue != null) { %>
					    <div class="p-a5">
						    <h4>Issues In Your <%= Model.LocalIssueLocation%></h4>
						    <div class="clear">&nbsp;</div>
						    Resident <a href="<%= HaveAVoice.Helpers.LinkHelper.Profile(Model.LocalIssue.User) %>" class="name"><%= HaveAVoice.Helpers.NameHelper.FullName(Model.LocalIssue.User)%></a> says, &quot;<%= Model.LocalIssue.Description%>&quot;<br />
                            In <%= Model.LocalIssue.User.Gender.ToUpper().Equals(HAVGender.Male.ToString().ToUpper()) ? "his" : "her"%> issue: <b><a href="<%= LinkHelper.IssueUrl(Model.LocalIssue.Title) %>" class="issue"><%= Model.LocalIssue.Title%></a></b>.
			            </div>
                    <% } %>
			        <div class="clear">&nbsp;</div>
				</div>
			<% } %>
		</div>
	</div>

    <% bool myIsAllowed = PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile); %>
    <% if (myIsAllowed) { %>
        <div class="col-6 round-3">
	        <div class="p-a10">
		        <h6 class="m-btm5"><% if (myUser != null && myUser.Details.Id == Model.User.Id) { %>My <% } %>Stats</h6>
		        <div class="col-1 teal fnt-14">
                    <% int myIssueCount = Model.User.Issues.Count; %>
                    <%= myIssueCount%>
                </div>
                <div class="col-4 c-white fnt-14">
                    <% if (myIssueCount == 1) { %>
                        Issue Raised
                    <% } else { %>
                        Issues Raised
                    <% } %>
                </div>
		        <div class="clear">&nbsp;</div>
		        <div class="col-6 fnt-12 p-v5">
		            <span class="green"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Like)%> agrees</span>
		            <span class="teal m-lft10 m-rgt10">|</span>
		            <span class="red"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Dislike)%> disagrees</span>
                </div>
                <div class="col-1 teal fnt-14">
                    <%  int myIssueReplyCount = Model.User.IssueReplys.Count; %>
            	    <%= myIssueReplyCount%>
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="col-4 c-white fnt-14">
                    <% if (myIssueReplyCount == 1) { %>
                        Idea Added
                    <% } else { %>
                        Ideas Added
                    <% } %>
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="clear">&nbsp;</div>
	            <div class="spacer-5">&nbsp;</div>

	            <div class="col-1 teal fnt-14">
                    <% int myFriends = Model.User.FriendedBy.Count; %>
		            <%= myFriends%>
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1 c-white fnt-14">
                    <% if (myFriends == 1) { %>
                        Friend
                    <% } else { %>
		                Friends
                    <% } %>
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-5">&nbsp;</div>
	            <div class="col-1 teal fnt-14">
                    <% int myFans = Model.User.FansOfMe.Count; %>
	        	    <%= myFans%>
	        	    <div class="clear">&nbsp;</div>
        	    </div>
        	    <div class="col-1 c-white fnt-14">
                    <% if (myFans == 1) { %>
	        	        Fan
                    <% } else { %>
                        Fans
                    <% } %>
	        	    <div class="clear">&nbsp;</div>
        	    </div>
        	    <div class="clear">&bnsp;</div>
	        </div>
	        <div class="clear">&nbsp;</div>
        </div>
    <% } %>
</div>
<div class="clear">&nbsp;</div>