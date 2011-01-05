<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<UserPicture>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.UserModel); %>
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
				<div class="col-4 pull-6 center">
					<input type="submit" value="Upload" class="create" />
				</div>
				<div class="clear">&nbsp;</div>
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>

        <% foreach (var item in Model.Models) { %>
			<%= /* ImageHelper.Image("/UserPictures/" + item.ImageName, 200, 200); */ %>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>