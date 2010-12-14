<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Feedback</h2><br /><br />
    <p>
        <%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>
    </p>
    <% using (Html.BeginForm()) {%>
        <p>
                 <%= Html.Encode(ViewData["Message"]) %>
        </p>
        <p>
            <%= Html.TextArea("Feedback", new { style = "width:300px; height: 200px" })%>
            <%= Html.ValidationMessage("Feedback", "*")%>
        </p>
            <p>
                <input type="submit" value="Send" />
            </p>
    <% } %>

</asp:Content>
