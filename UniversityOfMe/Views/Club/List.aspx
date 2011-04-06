<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<UniversityOfMe.Models.Club>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Id
            </th>
            <th>
                UniversityId
            </th>
            <th>
                ClubType
            </th>
            <th>
                CreatedByUserId
            </th>
            <th>
                Name
            </th>
            <th>
                Picture
            </th>
            <th>
                Description
            </th>
            <th>
                DateTimeStamp
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
                <%: item.UniversityId %>
            </td>
            <td>
                <%: item.ClubType %>
            </td>
            <td>
                <%: item.CreatedByUserId %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.Picture %>
            </td>
            <td>
                <%: item.Description %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.DateTimeStamp) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

