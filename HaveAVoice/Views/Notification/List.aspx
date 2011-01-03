<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Board>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Notifications</h2> <br /><br />
    <% foreach (var item in Model) { %>
        <%= Html.ActionLink(item.Message, "View", "Board", new { id = item.Id }, null)%><br />
    <% } %>

</asp:Content>
