<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<PhotoAlbum>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
    <% bool myIsUser = myUserInformationModel.Details.Id == Model.SourceUserIdOfContent; %>

    <% foreach (var item in Model.Models) { %>
        <%= Html.ActionLink(item.Name, "Details", "PhotoAlbum", new { id = item.Id }, null)%> 
        <% if (myIsUser) { %>
            <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = item.Id }, null)%>
            <%= Html.ActionLink("Delete", "Delete", "PhotoAlbum", new { id = item.Id }, null)%><br /> 
        <% } %>
    <% } %>

    <% if (myIsUser) { %>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.")%>

        <% using (Html.BeginForm("Create", "PhotoAlbum")) {%>
            <%= Html.Encode(ViewData["Message"])%><br />
            <%= Html.Encode(TempData["Message"])%><br />
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name")%>
                <%= Html.ValidationMessage("Name", "*")%>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextArea("Description")%>
                <%= Html.ValidationMessage("Description", "*")%>
            </p>
            <p>
                <input type="submit" value="Post" />
            </p>
        <% } %>
    <% } %>
</asp:Content>