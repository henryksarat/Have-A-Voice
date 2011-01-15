<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<Photo>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    <%= Html.Encode(ViewData["Message"]) %>
    <div class="clear">&nbsp;</div>
    
    <%= Html.ActionLink("Set to profile picture", "SetProfilePicture", "Photos", new { id = Model.Model.Id }, null) %>
    <div class="large-photo">
		<img src="<%= PhotoHelper.ConstructUrl(Model.Model.ImageName) %>" />
	</div>
	<div class="clear">&nbsp;</div>
</asp:Content>