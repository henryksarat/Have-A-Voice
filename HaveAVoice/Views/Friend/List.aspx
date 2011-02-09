<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Friend>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		$(function() {
			$("div.friend").mouseenter(function() {
				$("a.delete", $(this)).show();
			}).mouseleave(function() {
				$("a.delete", $(this)).hide();
			});
		});
	</script>
	
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>
        
    <div class="col-21">
        <% Html.RenderPartial("Message"); %>

    	<% int cnt = 0; %>
    	<% string klass = "friend"; %>
	    <% foreach (var item in Model.Models) { %>
	    	<% if (cnt % 2 == 0) {
	    		klass = "friend";
	    	} else {
	    		klass = "friend alt";
	    	}%>
	    	
			<div class="col-4 center <%= klass %>">
				<div class="p-a5">
					<div class="profile">
						<a href="/Friend/Delete/<%= item.FriendUserId %>" class="delete" title="Remove Friend">Remove Friend</a>
						<img src ="<%= PhotoHelper.ProfilePicture(item.FriendUser) %>" alt="<%= NameHelper.FullName(item.FriendUser) %>" class="profile" />
					</div>
					<div class="p-a5">
						<a href="<%= LinkHelper.Profile(item.FriendUser) %>" class="name"><%= NameHelper.FullName(item.FriendUser) %></a>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			
			<% if (cnt % 4 == 0) { %>
				<div class="clear">&nbsp;</div>
			<% } %>
			
	        <% cnt++; %>
	    <% } %>
	</div>
</asp:Content>
