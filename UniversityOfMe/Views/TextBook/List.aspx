<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<UniversityOfMe.Models.TextBook>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

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
                UniversityId
            </th>
            <th>
                TextBookConditionId
            </th>
            <th>
                BookTitle
            </th>
            <th>
                BookPicture
            </th>
            <th>
                ClassCode
            </th>
            <th>
                BuySell
            </th>
            <th>
                Edition
            </th>
            <th>
                Price
            </th>
            <th>
                Details
            </th>
            <th>
                DateTimeStamp
            </th>
            <th>
                Active
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
                <%: item.UniversityId %>
            </td>
            <td>
                <%: item.TextBookConditionId %>
            </td>
            <td>
                <%: item.BookTitle %>
            </td>
            <td>
                <%: item.BookPicture %>
            </td>
            <td>
                <%: item.ClassCode %>
            </td>
            <td>
                <%: item.BuySell %>
            </td>
            <td>
                <%: item.Edition %>
            </td>
            <td>
                <%: String.Format("{0:F}", item.Price) %>
            </td>
            <td>
                <%: item.Details %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.DateTimeStamp) %>
            </td>
            <td>
                <%: item.Active %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

