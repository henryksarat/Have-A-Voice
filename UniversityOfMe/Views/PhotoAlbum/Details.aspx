<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    Album info:<br />
    <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = Model.Get().Id }, null) %><br />
    <%= Html.ActionLink("Delete", "Delete", "PhotoAlbum", new { id = Model.Get().Id }, null)%><br /><br />
    <a href="<%= URLHelper.PhotoAlbumDetailsUrl(Model.Get()) %>"><img src="<%= PhotoHelper.PhotoAlbumCover(Model.Get()) %>" /></a><br />
    Name: <%= Model.Get().Name %><br />
    Description: <%= Model.Get().Description %><br /><br />

    Upload new photo: <br />

    <% using (Html.BeginForm("Create", "Photo", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
            <%= Html.Hidden("AlbumId", Model.Get().Id) %>
			<div class="col-4 push-6 center">
				<input type="file" id="ImageFile" name="ImageFile" size="23" />
			</div>
                
		<div class="col-4 push-8 center">
			<input type="submit" value="Upload" />
		</div>
	<% } %>

    Photos:<br />

    <% foreach (Photo myPhoto in Model.Get().Photos) { %>
        <a href="<%= URLHelper.PhotoDisplayUrl(myPhoto) %>"><img src="<%= PhotoHelper.ConstructUrl(myPhoto.ImageName) %>" /><br /></a>
    <% } %>
</asp:Content>