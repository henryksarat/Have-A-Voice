<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Friend>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Friend
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Friends</h2> <br /><br />
    <% foreach (var item in Model) { %>
        <%= item.FriendUser.Username %><br />
    <% } %>

</asp:Content>
