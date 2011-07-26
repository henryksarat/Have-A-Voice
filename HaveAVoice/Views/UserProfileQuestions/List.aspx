<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<FriendConnectionModel>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Friend Suggestions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>
        
    <div class="col-21">
        <div class="clear">&nbsp;</div>

        <div class="action-bar bold p-a10 m-btm20 color-4">
        	Friend Suggestions
        </div>

        <% Html.RenderPartial("Message"); %>

        <div class="clear">&nbsp;</div>

 	    <% foreach (FriendConnectionModel myFriendConnectionModel in Model.Get()) { %>
	        <div class="col-21 m-btm10">
	    	    <div class="col-2 center">
                    <a href="<%= LinkHelper.Profile(myFriendConnectionModel.User)%>">
	    		        <img src="<%= PhotoHelper.ProfilePicture(myFriendConnectionModel.User) %>" class="profile sm" />
                    </a>
	    		    <div class="clear">&nbsp;</div>
	    	    </div>
    		    <div class="col-13 m-lft m-rgt fnt-12 color-1">
                    <a class="name" href="<%= LinkHelper.Profile(myFriendConnectionModel.User)%>">
    			        <%= NameHelper.FullName(myFriendConnectionModel.User)%>
                    </a><br />
                    You two answered the question "<%= myFriendConnectionModel.QuestionConnectionMadeFrom.DisplayQuestion %>" in a similar way.
    			    <div class="clear">&nbsp;</div>
    		    </div>
    		    <div class="col-3 center">
                    <a class="button" href="<%= LinkHelper.AddFriend(myFriendConnectionModel.User, "UserProfileQuestions", "List") %>">Add Friend</a>
    			    <div class="clear">&nbsp;</div>
    		    </div>
    		    <div class="col-3 center">
                    <a class="button" href="<%= LinkHelper.IgnoreFriendSuggestion(myFriendConnectionModel.User.Id, "UserProfileQuestions", "List") %>">Ignore</a>
    			    <div class="clear">&nbsp;</div>
    		    </div>
    		    <div class="clear">&nbsp;</div>
    	    </div>
	    <% } %>
	</div>
</asp:Content>