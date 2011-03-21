<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.User>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">SEARCH</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
		<div class="col-24 m-btm10">
		    <% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
		    <% foreach (User myUser in Model) { %>
		    	<div class="col-6 center">
		    		<a href="<%= LinkHelper.Profile(myUser) %>">
		    			<img src="<%= PhotoHelper.ProfilePicture(myUser) %>" alt="<%= NameHelper.FullName(myUser) %>" class="profile" />
		    		</a>
		    		<br />
		    		<a class="name" href="<%= LinkHelper.Profile(myUser) %>">
						<%= NameHelper.FullName(myUser) %>
		    		</a><br />
		    		<div class="col-3 center m-top5">
		    			<%= Html.ActionLink("Message", "Create", "Message", new { id = myUser.Id }, new { @class = "button" }) %>
		    			<div class="clear">&nbsp;</div>
		    		</div>
		    		<div class="col-3 center m-top5">
				        <% if (myUserInfo!= null && !FriendHelper.IsFriend(myUserInfo.Details, myUser)) { %>
				            <%= Html.ActionLink("Add Friend", "Add", "Friend", new { id = myUser.Id }, new { @class = "button" })%><br />
				        <% } %>
		    		</div>
		    		<div class="clear">&nbsp;</div>
		    	</div>
		    <% } %>
    		<div class="clear">&nbsp;</div>
		</div>
</asp:Content>