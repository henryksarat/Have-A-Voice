<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Event>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Calendar</h2><br />
    <% Html.RenderPartial("Message"); %>

    <b>Events:</b><br /><br />
    <% foreach (var item in Model) { %>
        Date: <%=item.Date %> <br />
        Information: <%=item.Information %><br /><br />
    <% } %>
</asp:Content>
