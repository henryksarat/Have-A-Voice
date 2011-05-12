<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Albums
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <a href="/PhotoAlbum/Create">Create Photo Album</a><br /><br />

    Current albums:<br /><br />

    <% foreach(PhotoAlbum myPhotoAlbum in Model.Get()) { %>
        <a href="<%= URLHelper.PhotoAlbumDetailsUrl(myPhotoAlbum) %>"><img src="<%= PhotoHelper.PhotoAlbumCover(myPhotoAlbum) %>" /></a><br />
        Name: <%= myPhotoAlbum.Name %><br />
        Description: <%= myPhotoAlbum.Description %><br />
        <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = myPhotoAlbum.Id }, null) %><br />
        <%= Html.ActionLink("Delete", "Delete", "PhotoAlbum", new { id = myPhotoAlbum.Id }, null) %><br /><br />
    <% } %>
	
</asp:Content>