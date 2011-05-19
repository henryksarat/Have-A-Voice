<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>

<% User myUser = UserInformationFactory.GetUserInformation().Details; %>
<div class="banner red"> 
	<div class="profile"> 
		<img src="<%= PhotoHelper.ProfilePicture(myUser) %>" alt="<%= NameHelper.FullName(myUser) %>" title="<%= NameHelper.FullName(myUser) %>" class="profile sm" /> 
		Welcome <%= myUser.FirstName %>, <a href="/Authentication/LogOut">Logout</a> 
	</div> 
	<ul class="nav"> 
		<li class="mail"> 
			<a href="#" class="mail">Mail</a> 
			<span><%= NavigationCountHelper.NewMessageCount(myUser) %></span> 
		</li> 
		<li class="friend has-sub"> 
			<a href="#">Friends</a> 
			<span><%= NavigationCountHelper.PendingFriendCount(myUser) %></span> 
			<div class="sub-nav"> 
				<a href="#">All Friends</a> 
				<a href="#">Pending Friend Requests</a> 
			</div> 
		</li> 
		<li> 
			<a href="/PhotoAlbum/List">My Photos</a> 
		</li> 
		<li> 
			<a href="#">Account</a> 
		</li> 
	</ul> 
	<div class="clearfix"></div> 
	<span class="corner right"></span> 
</div> 