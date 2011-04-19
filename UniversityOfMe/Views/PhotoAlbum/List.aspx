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

    Current albums:<br /><br />

    <% foreach(PhotoAlbum myPhotoAlbum in Model.Get()) { %>
        <a href="<%= URLHelper.PhotoAlbumDetailsUrl(myPhotoAlbum) %>"><img src="<%= PhotoHelper.PhotoAlbumCover(myPhotoAlbum) %>" /></a><br />
        Name: <%= myPhotoAlbum.Name %><br />
        Description: <%= myPhotoAlbum.Description %><br />
        <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = myPhotoAlbum.Id }, null) %><br /><br />
    <% } %>
	
    <% using (Html.BeginForm("Create", "PhotoAlbum")) {%>
        <div class="editor-label">
            <%: Html.Label("Name") %>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Name")%>
            <%: Html.ValidationMessage("Name", "*")%>
        </div>

        <div class="editor-label">
            <%: Html.Label("Description") %>
        </div>
        <div class="editor-field">
            <%: Html.TextArea("Description", null, 10, 30, null)%>
            <%: Html.ValidationMessage("Description", "*")%>
        </div>

        <p>
            <input type="submit" value="Send" />
        </p>
    <% } %>
</asp:Content>