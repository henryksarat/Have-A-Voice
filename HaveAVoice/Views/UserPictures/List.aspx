<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<UserPicture>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <%= Html.Encode(ViewData["Message"]) %>
        <%= Html.Encode(TempData["Message"]) %>
        <div class="clear">&nbsp;</div>
        
		<div class="filter">
			<% using (Html.BeginForm("Add", "UserPictures", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) { %>
				<div class="col-4 push-6 center">
					<input type="file" id="ProfilePictureUpload" name="ProfilePictureUpload" size="23" />
	                <%= Html.ValidationMessage("ProfilePictureUpload", "*") %>
				</div>
				<div class="col-4 push-8 center">
					<input type="submit" value="Upload" class="create" />
				</div>
				<div class="clear">&nbsp;</div>
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="spacer-10">&nbsp;</div>

		<% int cnt = 0; %>
		<% string klass = "gallery"; %>
        <% foreach (var item in Model.Models) { %>
        	<% if (cnt % 2 == 0) {
        		klass = "gallery";
			} else {
				klass = "gallery alt";
        	} %>
			<div class="col-4 center <%= klass %>">
				<div class="p-a5">
					<div class="image">
						<a href="/UserPictures/Display/<%= item.Id %>" target="_blank">
							<img src="<%= PhotoHelper.ConstructUrl(item.ImageName) %>" alt="<%= item.ImageName %>" />
						</a>
					</div>
					<div class="p-a5">
						<a href="<%= PhotoHelper.ConstructUrl(item.ImageName) %>" target="_blank"><%= item.ImageName %></a>
					</div>
				</div>
			</div>
			<% cnt++; %>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>