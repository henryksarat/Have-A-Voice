<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.UserPicture>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% /* Html.RenderPartial("UserPanel", Model.UserModel); */ %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
	    <%= Html.Encode(ViewData["Message"]) %>
	    <div class="clear">&nbsp;</div>
        <% foreach (var item in Model) { %>
            <%= ImageHelper.Image("/UserPictures/" + item.ImageName, 200, 200)%>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>