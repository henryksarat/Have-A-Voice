<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Friend>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pending Friends
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
        	Pending Friend Requests
        </div>

        <% Html.RenderPartial("Message"); %>

        <div class="clear">&nbsp;</div>
	    <% foreach (var item in Model.Models) { %>
	    	<div class="col-21 m-btm10">
	    		<div class="col-2 center">
	    			<img src="/Photos/no_profile_picture.jpg" class="profile sm" />
	    			<div class="clear">&nbsp;</div>
	    		</div>
    			<div class="col-15 m-lft m-rgt fnt-12 bold color-1">
    				<%= HaveAVoice.Helpers.NameHelper.FullName(item.FriendUser) %>
    				<div class="clear">&nbsp;</div>
    			</div>
    			<div class="col-2 center">
    				<%= Html.ActionLink("Approve", "Approve", new { id = item.Id }, new { @class = "button" })%>
    				<div class="clear">&nbsp;</div>
    			</div>
    			<div class="col-2 center">
    				<%= Html.ActionLink("Decline", "Decline", new { id = item.Id }, new { @class = "button" })%>
    				<div class="clear">&nbsp;</div>
    			</div>
    			<div class="clear">&nbsp;</div>
    		</div>
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>

