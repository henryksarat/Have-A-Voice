<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<UniversityOfMe.Models.ProfessorReview>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

    <table>
        <tr>
            <th></th>
            <th>
                Id
            </th>
            <th>
                UserId
            </th>
            <th>
                ProfessorId
            </th>
            <th>
                AcademicTermId
            </th>
            <th>
                Year
            </th>
            <th>
                Class
            </th>
            <th>
                Rating
            </th>
            <th>
                Review
            </th>
            <th>
                DateTimeStamp
            </th>
            <th>
                Anonymous
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.Id })%>
            </td>
            <td>
                <%: item.Id %>
            </td>
            <td>
                <%: item.UserId %>
            </td>
            <td>
                <%: item.ProfessorId %>
            </td>
            <td>
                <%: item.AcademicTermId %>
            </td>
            <td>
                <%: item.Year %>
            </td>
            <td>
                <%: item.Class %>
            </td>
            <td>
                <%: item.Rating %>
            </td>
            <td>
                <%: item.Review %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.DateTimeStamp) %>
            </td>
            <td>
                <%: item.Anonymous %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

