<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Group>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>

<% foreach (HaveAVoice.Models.Group myGroup in Model) { %>    
    <a href="/Group/Details/<%= myGroup.Id %>"><%= myGroup.Name %></a>
<% } %>
</asp:Content>