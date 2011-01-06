<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<div class="col-24 user-panel">
	<div class="col-3">
		<img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.User.Username %>" class="profile" />
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
        <% string myHomeCss = Model.SiteSection == SiteSection.Home ? "first active" : "first"; %>
        <% string myMessageCss = Model.SiteSection == SiteSection.UserFeed ? "first active" : "message"; %>
        <% string myPhotosCss = Model.SiteSection == SiteSection.Photos ? "first active" : "photo"; %>
        <% string myCalendarCss = Model.SiteSection == SiteSection.Calendar ? "first active" : "event"; %>
        <% string myInfoCss = Model.SiteSection == SiteSection.Information ? "last active" : "last"; %>
			<ul>
				<li class="<%= myHomeCss %>"><a class="home" href="/Home/FanFeed">HOME</a></li>
				<li><a class="<%= myMessageCss %>" href="/Home/UserFeed">My Feed</a></li>
				<li><a class="<%= myPhotosCss %>" href="/UserPictures/List">PHOTOS</a></li>
				<li><a class="<%= myCalendarCss %>" href="/Calendar/List">EVENTS</a></li>
				<li class="<%= myInfoCss %>"><a class="info" href="#">INFO</a></li>
			</ul>
		</div>
		<div class="clear">&nbsp;</div>

		<div class="m-lft col-15 m-rgt user-control">
			<h1>
				<%= Model.User.Username %>
			</h1>
			<h2>
				#USER-STATUS#
				<!--
				I'm not being lazy just energy efficent
				//-->
			</h2>
			<span class="time">
				#USER-TIMESTAMP#
				<!--
				3:54PM from Twitter
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