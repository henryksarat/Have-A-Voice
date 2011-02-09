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
        <% Html.RenderPartial("Message"); %>
	
	    <% foreach (var item in Model.Models) { %>
	    	<div class="col-7">
	    		<div class="col-2">
	    			<img src="/Content/images/no_profile_picture.jpg" class="profile" />
	    			<div class="clear">&nbsp;</div>
	    		</div>
	    		<div class="col-5">
	    			<div class="col-5 m-lft fnt-12 bold color-5">
	    				<%= HaveAVoice.Helpers.NameHelper.FullName(item.FriendUser) %>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="col-2 m-lft">
	    				<%= Html.ActionLink("Approve", "Approve", new { id = item.Id }, new { @class = "approve" })%>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-1">&nbsp;</div>
	    			<div class="col-2">
	    				<%= Html.ActionLink("Decline", "Decline", new { id = item.Id }, new { @class = "decline" })%>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    		</div>
	    		<div class="clear">&nbsp;</div>
	    	</div>
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>

