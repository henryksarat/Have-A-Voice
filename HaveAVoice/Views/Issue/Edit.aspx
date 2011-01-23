<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditIssueReply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("Message"); %>

    <h2>Edit your issue reply</h2>

    <% using (Html.BeginForm()) { %>
        <%= Html.ValidationSummary("Your issue wasn't edited. Please correct your errors and try again.") %>
        <p>
            <%= Html.Encode(ViewData["Message"])%>
        </p>
        <p>
            <%= Html.TextBox("Title", Model.Title)%>
            <%= Html.ValidationMessage("Title", "*") %>
        </p>
        <p>
            <%= Html.TextArea("Description", Model.Description, 5, 30, null)%>
            <%= Html.ValidationMessage("Description", "*")%>
        </p>
        <p>
            <input type="submit" value="Submit" />
        </p>
    <% } %>

</asp:Content>
