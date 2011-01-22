<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.IssueReplyWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditIssueReply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit your issue reply</h2>

    <% using (Html.BeginForm()) { %>
        <%= Html.ValidationSummary("Your reply wasn't posted. Please correct your errors and try again.") %>
        <p>
            <%= Html.Encode(ViewData["Message"]) %>
        </p>

        <p>
            <%= Html.TextArea("Body", Model.Body, 5, 30, null)%>
            <%= Html.ValidationMessage("Body", "*")%>
        </p>
        <table>
            <tr>
                <td><label for="Like">Like</label> <%= Html.RadioButton("Disposition", (int)Disposition.Like, Model.Disposition == (int)Disposition.Like ? true : false)%></td>
                <td><label for="Dislike">Dislike</label> <%= Html.RadioButton("Disposition", (int)Disposition.Dislike, Model.Disposition == (int)Disposition.Dislike ? true : false)%></td>
                <td><%= Html.ValidationMessage("Disposition", "*")%></td>
            </tr>
        </table>

        <p>
            <input type="submit" value="Submit" />
        </p>
    <% } %>

</asp:Content>
