<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoDisplayView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Photo details: <br />
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>    
	    <% UserInformationModel<User> myUserInformationModel = UserInformationFactory.GetUserInformation(); %>
	    <% if (myUserInformationModel.Details.Id == Model.Get().Photo.UploadedByUserId) { %>
	        <%= Html.ActionLink("Set as profile picture", "SetProfilePicture", "Photo", new { id = Model.Get().Photo.Id }, null)%>
            <%= Html.ActionLink("Set as album cover", "SetAlbumCover", "Photo", new { id = Model.Get().Photo.Id }, null)%>
            <%= Html.ActionLink("Delete", "Delete", "Photo", new { id = Model.Get().Photo.Id, albumId = Model.Get().Photo.PhotoAlbumId }, null)%>
	    <% } %>
        <br />

        <% if (Model.Get().PreviousPhoto != null) { %>
            <a href="/Photo/Display/<%= Model.Get().PreviousPhoto.Id %>">Previous</a>
        <% } %>
        <img src="<%= PhotoHelper.ConstructUrl(Model.Get().Photo.ImageName) %>" />
        <% if (Model.Get().NextPhoto != null) { %>
            <a href="/Photo/Display/<%= Model.Get().NextPhoto.Id %>">Next</a>
        <% } %><br /><br />
    Write comment:<br />
    <% using (Html.BeginForm("Create", "PhotoComment", new { id = Model.Get().Photo.Id }, FormMethod.Post, null)) { %>
        <div class="editor-field">
            <%: Html.TextArea("Comment", null, 10, 30, null)%>
            <%: Html.ValidationMessage("Comment", "*")%>
        </div>

		<input type="submit" value="Comment" />
	<% } %><br /><br />

    Comments: <br />
    <% foreach (PhotoComment myComment in Model.Get().Photo.PhotoComments) { %>
        <%= NameHelper.FullName(myComment.User) %><br />
        <%= myComment.DateTimeStamp %><br />
        <%= myComment.Comment %><br />
        -----------------<br />
    <% } %>
</asp:Content>