<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<div class="col-24 user-panel">
	<div class="col-3 center">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.User.Username %>" class="profile" />
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
        
            <%= NavigationHelper.UserNavigation(
                    Model.SiteSection, 
                    new SiteSection[] {SiteSection.Home, SiteSection.IssueActivity, SiteSection.Photos, SiteSection.Calendar},
                    new string[] {"home", "message", "photo", "event"},
                    new string[] {"/Profile/Show", "/Profile/IssueActivity", "/PhotoAlbum/List", "/Calendar/List"},
                    new string[] { "HOME", "USER FEED", "PHOTOS", "EVENTS"}
                )
            %>
		</div>
		<div class="clear">&nbsp;</div>

		<div class="m-lft col-15 m-rgt user-control">
			<h1>
				<%= Model.User.FirstName + " " + Model.User.LastName %>
			</h1>
			<h2>
				<!--
					#USER-STATUS#
				//-->
			</h2>
			<span class="time">
				<!--
					#USER-TIMESTAMP#
				//-->
			</span>
		</div>
	</div>

    <% if (HAVUserInformationFactory.GetUserInformation().Details.Id != Model.User.Id) { %>
	    <div class="col-6 round-3">
		    <div class="p-a10">
			    <input type="button" class="fan" value="Become a friend" />
			    <a class="p-v5 m-top15" href="#">Send <%=Model.User.FirstName%> a private message</a>
			    <h6 class="m-top15">Stats</h6>
			    <hr />
			    <div class="col-1 teal fnt-18">32</div><div class="col-1 c-white fnt-18">Ideas</div>
			    <div class="clear">&nbsp;</div>
			    <div class="col-6 fnt-12 p-v10">
			        <span class="green">102 likes</span>
			        <span class="teal m-lft10 m-rgt10">|</span>
			        <span class="red">1024 dislikes</span>
                </div>
		        <div class="clear">&nbsp;</div>
		        <div class="col-1 teal fnt-18"><%=Model.User.FriendedBy.Count%></div><div class="col-1 c-white fnt-18">Friends</div>
		        <div class="clear">&nbsp;</div>
		    </div>
        </div>
    <% } %>
</div>
<div class="clear">&nbsp;</div>