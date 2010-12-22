<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (HAVUserInformationFactory.IsLoggedIn()) { %>

<div class="col-24 user-panel">
	<div class="col-3">
		#USER-IMAGE#
		<!--
		<img src="http://www2.pictures.zimbio.com/img/5519/Alicia/5986c.nZBops.jpg" alt="Gerard Butler" width="120" height="120" class="profile" />
		//-->
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
			<ul>
				<li class="first active"><a class="home" href="#">HOME</a></li>
				<li><a class="message" href="#">MESSAGE</a></li>
				<li><a class="photo" href="#">PHOTOS</a></li>
				<li><a class="event" href="#">EVENTS</a></li>
				<li class="last"><a class="info" href="#">INFO</a></li>
			</ul>
		</div>
		<div class="clear">&nbsp;</div>
		
		<div class="m-lft col-15 m-rgt user-control">
			<h1>
				#USER-NAME#
				<!--
				Gerard Butler
				//-->
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

<% } else { %>

<div class="push-1 col-22 sign-up">
	<div class="col-16 center padding-22t">
		<h3>
			Connecting you to the political world instantaneously.
		</h3>
	</div>
	
	<div class="col-6 center">
		<input type="button" class="create" value="Create a new account" />
	</div>
	<div class="clear">&nbsp;</div>
</div>

<% } %>
<div class="clear">&nbsp;</div>