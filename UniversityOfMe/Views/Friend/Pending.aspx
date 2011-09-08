<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Friend>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pending Friend Requests
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="banner title black full red-top small"> 
			<span>Pending Friend Requests</span> 
		</div> 

        <ul id="list" class="friend-list clearfix"> 
            <% foreach (Friend myFriend in Model.Get()) { %>
			    <li> 
				    <div id="<%= myFriend.FriendedUser.FirstName %>"> 
					    <a href="<%= URLHelper.ProfileUrl(myFriend.FriendedUser) %>"><img src="<%= PhotoHelper.ProfilePicture(myFriend.FriendedUser) %>" class="profile lrg flft mr9" /></a>
					    <span class="name"><%= NameHelper.FullName(myFriend.FriendedUser) %></span>
					    <%= UniversityHelper.GetMainUniversity(myFriend.FriendedUser).UniversityName %><br /> 
					    <span class="red">&nbsp;</span> 
                        <a href="/Friend/Decline/<%= myFriend.Id %>" class="frgt deny">Remove</a>
                        <a href="/Friend/Approve/<%= myFriend.Id %>" class="frgt approve">Remove</a>
				    </div> 
			    </li> 
            <% } %>
            <% if(Model.Get().Count() == 0) { %>
                <div class="center small bold">
                    You have no pending friend requests.
                </div>
            <% } %>
		</ul> 
        </div>
</asp:Content>

