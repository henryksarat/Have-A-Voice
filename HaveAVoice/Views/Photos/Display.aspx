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
	        <%= Html.ActionLink("Set as profile picture", "SetProfilePicture", "Photos", new { id = Model.Model.Id }, new { @class = "button"}) %>
            <%= Html.ActionLink("Set as album cover", "SetAlbumCover", "Photos", new { id = Model.Model.Id }, new { @class = "button" })%>
            <%= Html.ActionLink("Delete", "Delete", "Photos", new { id = Model.Model.Id }, new { @class = "button delete" }) %>
	    <% } %>
	    <div class="large-photo m-top10">
			<img src="<%= PhotoHelper.ConstructUrl(Model.Model.ImageName) %>" />
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>