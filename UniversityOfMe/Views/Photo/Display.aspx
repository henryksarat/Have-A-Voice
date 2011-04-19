<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Photo>>" %>
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
	    <% if (myUserInformationModel.Details.Id == Model.Get().UploadedByUserId) { %>
	        <%= Html.ActionLink("Set as profile picture", "SetProfilePicture", "Photo", new { id = Model.Get().Id }, null) %>
            <%= Html.ActionLink("Set as album cover", "SetAlbumCover", "Photo", new { id = Model.Get().Id }, null)%>
            <%= Html.ActionLink("Delete", "Delete", "Photo", new { id = Model.Get().Id, albumId = Model.Get().PhotoAlbumId }, null)%>
	    <% } %>
        <br />

        <img src="<%= PhotoHelper.ConstructUrl(Model.Get().ImageName) %>" /><br />
</asp:Content>