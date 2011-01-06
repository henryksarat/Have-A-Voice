<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Fan>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.UserModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
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
						<img src ="<%= ProfilePictureHelper.ProfilePicture(item.FanUser) %>" alt="<%= item.FanUser.Username %>" class="profile" />
					</div>
					<div class="p-a5">
						<a href="#" class="name"><%= item.FanUser.Username %></a>
					</div>
				</div>
			</div>
			
			<% if (cnt % 4 == 0) { %>
				<div class="clear">&nbsp;</div>
			<% } %>
			
	        <% cnt++; %>
	    <% } %>
	</div>
</asp:Content>
