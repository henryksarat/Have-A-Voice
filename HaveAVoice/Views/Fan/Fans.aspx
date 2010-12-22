<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Fan>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Fans of me</h2> <br /><br />
    <% foreach (var item in Model) { %>
        <%= item.FanUser.Username %><br />
    <% } %>

</asp:Content>
