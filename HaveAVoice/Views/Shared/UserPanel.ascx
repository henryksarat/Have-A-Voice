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
            <% if (PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile)) { %>
            <%= NavigationHelper.UserNavigation(
                    Model.SiteSection,
                    new SiteSection[] { SiteSection.Home, SiteSection.IssueActivity, SiteSection.Photos, SiteSection.Calendar },
                    new string[] { "home", "message", "photo", "event" },
                    new string[] { "/Profile/Show", "/Profile/IssueActivity", "/PhotoAlbum/List", "/Calendar/List" },
                    new string[] { "HOME", "USER FEED", "PHOTOS", "EVENTS" },
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
			
            <% if (Model.SiteSection == SiteSection.Profile) { %>
                <% if (!FriendHelper.IsFriend(Model.User, myUser)) { %>
            	    <div class="col-6 m-top10">
			            <input type="button" class="fan" value="Become a Friend" />
			            <div class="clear">&nbsp;</div>
		            </div>
		            <div class="clear">&nbsp:</div>
                <% } %>
                <% if ((myUser.Id != Model.User.Id) && (FriendHelper.IsFriend(Model.User, myUser))) { %>
			        <a class="p-v5 m-top10 msg" href="/Message/Create/<%= Model.User.Id %>">Send <%= Model.User.FirstName%> a private message</a>
			    <% } %>
            <% } %>
		</div>
	</div>

    <% if (Model.SiteSection == SiteSection.Profile) { %>
        <div class="col-6 round-3">
	        <div class="p-a10">
		        <h6 class="m-btm5"><% if (myUser.Id == Model.User.Id) { %>My <% } %>Stats</h6>
		        <div class="col-1 teal fnt-14"><%= Model.User.Issues.Count%></div><div class="col-4 c-white fnt-14">Issues Raised</div>
		        <div class="clear">&nbsp;</div>
		        <div class="col-6 fnt-12 p-v5">
		            <span class="green"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Like)%> likes</span>
		            <span class="teal m-lft10 m-rgt10">|</span>
		            <span class="red"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Dislike)%> dislikes</span>
                </div>
                <div class="col-1 teal fnt-14">
            	    <%= Model.User.IssueReplys.Count%>
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="col-4 c-white fnt-14">
            	    Ideas Added
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="clear">&nbsp;</div>
	            <div class="spacer-5">&nbsp;</div>
	            <div class="col-1 teal fnt-14">
		            <%=Model.User.FriendedBy.Count%>
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1 c-white fnt-14">
		            Friends
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-5">&nbsp;</div>
	            <div class="col-1 teal fnt-14">
	        	    <%= Model.User.FansOfMe.Count %>
	        	    <div class="clear">&nbsp;</div>
        	    </div>
        	    <div class="col-1 c-white fnt-14">
	        	    Fans
	        	    <div class="clear">&nbsp;</div>
        	    </div>
        	    <div class="clear">&bnsp;</div>
	        </div>
	        <div class="clear">&nbsp;</div>
    </div>
    <% } %>
</div>
<div class="clear">&nbsp;</div>