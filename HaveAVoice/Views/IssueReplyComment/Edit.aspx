<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.IssueReplyComment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit your comment</h2>

    <% using (Html.BeginForm()) { %>
        <%= Html.ValidationSummary("Your comment wasn't posted. Please correct your errors and try again.") %>
        <p>
            <%= Html.Encode(ViewData["Message"]) %>
        </p>

        <p>
            <%= Html.TextBox("Comment", Model.Comment)%>
            <%= Html.ValidationMessage("Comment", "*") %>
        </p>
        <p>
            <input type="submit" value="Submit" />
        </p>
    <% } %>
</asp:Content>

