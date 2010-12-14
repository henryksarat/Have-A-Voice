<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.View.IssueWithDispositionModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>

    <table>
        <tr>
            <th>
                DateTimeStamp
            </th>
            <th>
                Title
            </th>
            <th>
                Description
            </th>
            <th>
                Disposition
            </th>
            <th>
                Deleted
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink(DateHelper.ConvertToLocalTime(item.Issue.DateTimeStamp).ToString(), "View", new { id = item.Issue.Id })%>
            </td>
            <td>
                <%= Html.ActionLink(item.Issue.Title, "View", new { id = item.Issue.Id })%>
            </td>
            <td>
                <%= Html.ActionLink(item.Issue.Description, "View", new { id = item.Issue.Id })%>
            </td>
            <td>
                <% if (!item.HasDisposition) { %>
                    <%= Html.ActionLink("Like", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.LIKE })%>
                    <%= Html.ActionLink("Dislike", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.DISLIKE })%>
                <% } %>
            </td>
            <td>
                <% if (item.Issue.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id) { %>
                    <%= Html.ActionLink("Delete", "DeleteIssue", new { deletingUserId = HAVUserInformationFactory.GetUserInformation().Details.Id, issueId = item.Issue.Id} ) %>
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

