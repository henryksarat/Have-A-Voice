<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel<Board>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Notifications</h2> <br /><br />

    <p>
        <%= Html.Encode(ViewData["Message"]) %><br /><br />
    </p>
    
    <% foreach (var item in Model.Models) { %>
        <%= Html.ActionLink("Now activity with this board message:" + item.Message, "View", "Board", new { id = item.Id }, null)%><br />
    <% } %>

</asp:Content>
