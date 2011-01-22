<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<Photo>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

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
	    
	    <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
	    <% if (myUserInformationModel.Details.Id == Model.Model.UploadedByUserId) { %>
		        <%= Html.ActionLink("Set to profile picture", "SetProfilePicture", "Photos", new { id = Model.Model.Id }, null) %><br />
		        <%= Html.ActionLink("Delete", "Delete", "Photos", new { id = Model.Model.Id }, null) %><br />
                <%= Html.ActionLink("Set to album cover", "SetAlbumCover", "Photos", new { id = Model.Model.Id }, null)%><br />
	    <% } %>
	    <div class="large-photo">
			<img src="<%= PhotoHelper.ConstructUrl(Model.Model.ImageName) %>" />
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>