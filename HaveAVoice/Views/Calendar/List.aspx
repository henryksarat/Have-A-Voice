<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Event>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Calendar</h2><br />
    <p>
            <%= Html.Encode(ViewData["Message"]) %>
    </p>
    <b>Events:</b><br /><br />
    <% foreach (var item in Model) { %>
        Date: <%=item.Date %> <br />
        Information: <%=item.Information %><br />
        <%= Html.ActionLink("Delete", "DeleteEvent", new { eventId = item.Id }) %><br /><br />
    <% } %>
    <b>Add event:</b> <br />
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %><br /><br />
    <% using (Html.BeginForm("AddEvent", "Calendar")) { %>
        Date: <%= Html.TextBox("Date", DateTime.UtcNow.AddMinutes(1))%>
        <%= Html.ValidationMessage("Date", "*") %><br />
        Information: <%= Html.TextArea("Information")%>
        <%= Html.ValidationMessage("Information", "*")%><br />
        <input type="submit" value="Create" /><br />
    <% } %>

</asp:Content>
