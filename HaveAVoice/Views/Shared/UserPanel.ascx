<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<div class="col-24 user-panel">
    <% User myUser = HAVUserInformationFactory.GetUserInformation().Details; %>
	<div class="col-3 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.FullName %>" class="profile" />
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
            <% if (Model.SiteSection == SiteSection.MyProfile || PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile)) { %>
            <% string myHomeText = Model.FullName + " profile page";  %>
            <% string myActivity = "issues that the user has participated in"; %>
            <% string myPhotos = "user's photos"; %>
            <% string myEvents = "user's events"; %>

            <% if(Model.SiteSection == SiteSection.MyProfile) { %>
                <% myHomeText = "my homepage";  %>
                <% myActivity = "issues I am participating in"; %>
                <% myPhotos = "my photos"; %>
                <% myEvents = "my events"; %>
            <% } %>
            <%= NavigationHelper.UserNavigation(
                    Model.SiteSection,
                    new SiteSection[] { SiteSection.Home, SiteSection.IssueActivity, SiteSection.Photos, SiteSection.Calendar },
                    new string[] { "home", "message", "photo", "event" },
                    new string[] { "/Profile/Show", "/Profile/IssueActivity", "/PhotoAlbum/List", "/Calendar/List" },
                    new string[] { myHomeText, myActivity, myPhotos, myEvents },
                    Model.User
                )
            %>
            <% } else { %>
            <%= NavigationHelper.UserNavigation(
                    Model.SiteSection,
                    new SiteSection[] { SiteSection.Home, SiteSection.IssueActivity, SiteSection.Photos, SiteSection.Calendar },
                    new string[] { "home-grey", "message-grey", "photo-grey", "event-grey" },
                    new string[] { "#", "#", "#", "#" },
                    new string[] { "user privacy settings disallow that", "user privacy settings disallow that", "user privacy settings disallow that", "user privacy settings disallow that" },
                    Model.User
                )
            %>
            <% } %>

            <% if (Model.SiteSection == SiteSection.Profile && (myUser.Id != Model.User.Id)) { %>
                <% if (!FanHelper.IsFan(Model.User.Id, myUser)) { %>
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
            <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>

		<div class="m-lft col-15 m-rgt user-control">
			<h1>
				<%= Model.User.FirstName + " " + Model.User.LastName %>
			</h1>
			<% bool myIsAuthority = RoleHelper.IsPolitician(Model.User) || RoleHelper.IsPoliticalCandidate(Model.User); %>

            <% if (Model.SiteSection == SiteSection.Profile) { %>
                <% if (!FriendHelper.IsFriend(Model.User, myUser) && !myIsAuthority) { %>
            	    <div class="col-6 m-top10">
            	    	<% using (Html.BeginForm("Add", "Friend", new { id = Model.User.Id })) { %>
				            <input type="submit" class="fan" value="Become a Friend" />
			            <% } %>
			            <div class="clear">&nbsp;</div>
		            </div>
		            <div class="clear">&nbsp:</div>
                <% } %>
                <% if (myUser.Id != Model.User.Id) { %>
			        <a class="p-v5 m-top10 msg" href="/Message/Create/<%= Model.User.Id %>">Send <%= Model.User.FirstName%> a private message</a>
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
                            In <%= Model.LocalIssue.User.Gender.ToUpper().Equals(HAVGender.Male.ToString().ToUpper()) ? "his" : "her"%> issue: <b><a href="/Issue/View/<%= Model.LocalIssue.Id %>" class="issue"><%= Model.LocalIssue.Title%></a></b>.
			            </div>
                    <% } %>
			        <div class="clear">&nbsp;</div>
				</div>
			<% } %>
		</div>
	</div>

        <div class="col-6 round-3">
	        <div class="p-a10">
		        <h6 class="m-btm5"><% if (myUser.Id == Model.User.Id) { %>My <% } %>Stats</h6>
		        <div class="col-1 teal fnt-14">
                    <% int myIssueCount = Model.User.Issues.Count; %>
                    <%= myIssueCount %>
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
            	    <%= myIssueReplyCount %>
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
		            <%= myFriends %>
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
	        	    <%= myFans %>
	        	    <div class="clear">&nbsp;</div>
        	    </div>
        	    <div class="col-1 c-white fnt-14">
                    <% if(myFans == 1) { %>
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
</div>
<div class="clear">&nbsp;</div>