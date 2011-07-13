﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>

<% User myUser = UserInformationFactory.GetUserInformation().Details; %>
<div class="banner red"> 
	<div class="profile"> 
		<a href="<%= URLHelper.ProfileUrl(myUser) %>"><img src="<%= PhotoHelper.ProfilePicture(myUser) %>" alt="<%= NameHelper.FullName(myUser) %>" title="<%= NameHelper.FullName(myUser) %>" class="profile sm" /></a> 
		Welcome <%= myUser.FirstName %>, <a href="/Authentication/LogOut">Logout</a> 
	</div> 
	<ul class="nav"> 
		<li class="mail"> 
			<a href="/Message/Inbox" class="mail">Mail</a> 
            <% int myMessageCount = NavigationCountHelper.NewMessageCount(myUser); %>
            <% if (myMessageCount != 0) { %>
			    <span><%= NavigationCountHelper.NewMessageCount(myUser)%></span> 
            <% } %>
		</li> 
		<li class="friend has-sub"> 
			<a href="#">Friends</a> 
            <% int myFriendRequestCount = NavigationCountHelper.PendingFriendCount(myUser); %>
            <% if (myFriendRequestCount != 0) { %>
			    <span><%= myFriendRequestCount %></span> 
            <% } %>
			<div class="sub-nav"> 
				<a href="/Friend/List">All Friends</a> 
				<a href="/Friend/Pending">Pending Friend Requests</a> 
			</div> 
		</li> 
		<li> 
			<a href="/PhotoAlbum/List">My Photos</a> 
		</li> 
		<li class="has-sub-account"> 
			<a href="#">Account</a> 
			<div class="sub-nav"> 
				<a href="/User/Edit">Account Settings</a> 
				<a href="/UserPrivacySetting/Edit">Privacy Settings</a> 
                <a href="/Feature/Edit">Site Feature Settings</a> 
			</div> 
		</li> 
	</ul> 
	<div class="clearfix"></div> 
	<span class="corner right"></span> 
</div> 