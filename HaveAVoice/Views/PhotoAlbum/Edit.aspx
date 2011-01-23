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
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <% Html.RenderPartial("Message"); %>

        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
        <div class="clear">&nbsp;</div>

        <% using (Html.BeginForm("Edit", "PhotoAlbum", FormMethod.Post, new { @class = "create" })) { %>
            <%= Html.Hidden("AlbumId", Model.Model.Id) %>
			<div class="col-6 m-rgt right">
				<label for="Name">Name:</label>
			</div>
			<div class="col-6">
				<%= Html.TextBox("Name", Model.Model.Name) %>
			</div>
			<div class="clear">&nbsp;</div>
			<div class="col-12 m-btm10">
				<%= Html.ValidationMessage("Name", "*") %>
			</div>
			<div class="clear">&nbsp;</div>

        	<div class="col-6 m-rgt right">
                <label for="Description">Description:</label>
            </div>
            <div class="col-6">
                <%= Html.TextArea("Description", Model.Model.Description, new { cols = "31", rows = "4", resize = "none"})%>
			</div>
			<div class="clear">&nbsp;</div>
			<div class="col-12 m-btm10">
				<%= Html.ValidationMessage("Description", "*")%>
			</div>
			<div class="clear">&nbsp;</div>

			<div class="m-btm30">
				<input type="submit" value="Edit" class="create" />
	            <%= Html.ActionLink("Cancel", "List", "", new { @class = "cancel" }) %>
			</div>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>