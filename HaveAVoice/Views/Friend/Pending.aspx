<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Friend>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pending Friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% /* Html.RenderPartial("UserPanel", Model.NavigationModel); */ %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
	    <%= Html.Encode(ViewData["Message"]) %>
	    <%= Html.Encode(TempData["Message"]) %>
		<div class="clear">&nbsp;</div>
	
	    <% foreach (var item in Model) { %>
	    	<div class="col-7">
	    		<div class="col-2">
	    			<img src="/Content/images/no_profile_picture.jpg" class="profile" />
	    		</div>
	    		<div class="col-5">
	    			<div class="col-5 m-lft fnt-12 bold color-5">
	    				<%= item.FriendUser.Username %>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="col-2 m-lft">
	    				<%= Html.ActionLink("Approve", "Approve", new { id = item.Id }, new { @class = "approve" })%>
	    			</div>
	    			<div class="col-1">&nbsp;</div>
	    			<div class="col-2">
	    				<%= Html.ActionLink("Decline", "Decline", new { id = item.Id }, new { @class = "decline" })%>
	    			</div>
	    		</div>
	    	</div>
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>

