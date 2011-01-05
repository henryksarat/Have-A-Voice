<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Fan>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.UserModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
	    <h2>Fans of me</h2> <br /><br />
	    <% foreach (var item in Model.Models) { %>
	        <%= item.FanUser.Username %><br />
	    <% } %>
	</div>
</asp:Content>
