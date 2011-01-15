<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<PhotoAlbum>>" %>
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
    
    <div class="col-21">
        <%= Html.Encode(ViewData["Message"]) %>
        <%= Html.Encode(TempData["Message"]) %>
        <div class="clear">&nbsp;</div>
        
        <div>
            Current album title = <%= Model.Model.Name %>
        </div>
        <div>
            Current album description = <%= Model.Model.Description %>
        </div>
   

        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

        <% using (Html.BeginForm("Edit", "PhotoAlbum")) {%>
            <%= Html.Hidden("AlbumId", Model.Model.Id) %>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name") %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextArea("Description")%>
                <%= Html.ValidationMessage("Description", "*")%>
            </p>
            <p>
                <input type="submit" value="Edit" />
            </p>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>