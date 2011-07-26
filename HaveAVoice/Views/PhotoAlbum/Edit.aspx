<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>
    
    <div class="col-21">
        <div class="action-bar bold p-a10 m-btm20 color-4">
        	Edit Photo Album: <%= Model.Get().Name %>
        </div>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <div class="clear">&nbsp;</div>

        <% using (Html.BeginForm("Edit", "PhotoAlbum", FormMethod.Post, new { @class = "create" })) { %>
            <%= Html.Hidden("AlbumId", Model.Model.Id) %>
			<div class="col-6 m-rgt right">
				<label for="Name">Name:</label>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="col-6">
				<%= Html.TextBox("Name", Model.Model.Name) %>
				<span class="req">
					<%= Html.ValidationMessage("Name", "*") %>
				</span>
				<div class="clear">&nbsp;</div>
			</div>

			<div class="clear">&nbsp;</div>
			<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>

        	<div class="col-6 m-rgt right">
                <label for="Description">Description:</label>
                <div class="clear">&nbsp;</div>
            </div>
            <div class="col-6">
                <%= Html.TextArea("Description", Model.Model.Description, new { cols = "31", rows = "4", resize = "none"}) %>
                <span class="req">
                	<%= Html.ValidationMessage("Description", "*") %>
                </span>
                <div class="clear">&nbsp;</div>
			</div>

			<div class="clear">&nbsp;</div>
			<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>

			<div class="col-6 m-btm30 push-6">
				<input type="submit" value="Edit" class="create" />
	            <%= Html.ActionLink("Cancel", "List", "PhotoAlbum", new { @class = "cancel" }) %>
	            <div class="clear">&nbsp;</div>
			</div>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>