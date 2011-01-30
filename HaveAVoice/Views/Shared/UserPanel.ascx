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
	<div class="col-6 advertisement">
		&nbsp;
		<!--
			<h1>ADVERTISEMENT</h1>
			<img src="http://pornmoviestream.com/enter/wp-content/gallery/big-tits-in-sports-4/thumbs/thumbs_10108042_p3.jpg" alt="Advertisement" />
			<b>Lorem ipsum</b>
			<br />
			Lorem ipsum dolor sit amet, consectetur adipiscing elit.
			Phasellus ipsum quam, varius non vehicula sit amet, aliquet a ante.
			Aenean dictum purus magna.
			Sed tempor ipsum pretium metus consectetur consequat.
		//-->
	</div>
</div>
<div class="clear">&nbsp;</div>