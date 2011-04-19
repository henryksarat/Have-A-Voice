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

    <% using (Html.BeginForm("Edit", "PhotoAlbum", FormMethod.Post)) { %>
        <%= Html.Hidden("AlbumId", Model.Get().Id) %>
        <div class="editor-label">
            <%: Html.Label("Name") %>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Name", Model.Get().Name)%>
            <%: Html.ValidationMessage("Name", "*")%>
        </div>

        <div class="editor-label">
            <%: Html.Label("Description") %>
        </div>
        <div class="editor-field">
            <%: Html.TextArea("Description", Model.Get().Description, 10, 30, null)%>
            <%: Html.ValidationMessage("Description", "*")%>
        </div>

        <p>
            <input type="submit" value="Submit" />
        </p>
    <% } %>
</asp:Content>