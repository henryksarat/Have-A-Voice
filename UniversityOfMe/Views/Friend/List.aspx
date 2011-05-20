<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Friend>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% foreach (Friend myFriend in Model.Get()) { %>
        <img src="<%= PhotoHelper.ProfilePicture(myFriend.FriendedUser) %>" />
        <a href="/<%= myFriend.FriendedUser.ShortUrl %>"><%= NameHelper.FullName(myFriend.FriendedUser) %></a><br />
        <%= UniversityHelper.GetMainUniversity(myFriend.FriendedUser) %><br />
        <% using (Html.BeginForm("Delete", "Friend", new { id = myFriend.FriendedUser.Id }, FormMethod.Post, null)) { %>
        			<div class="input"> 
				<input type="submit" name="submit" class="btn" value="UnFriend" /> 
			</div> 
        <% } %>

        <%= Html.ActionLink("Send Message", "Create", "Message", new {id = myFriend.FriendedUser.Id}, null) %>

        
    <% } %>
</asp:Content>

