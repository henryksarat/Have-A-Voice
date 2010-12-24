<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Fan>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pending
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Pending</h2>

    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
    <p>
        <%= Html.Encode(TempData["Message"]) %>
    </p>

    <table>
        <tr>
            <th>
                Fan username
            </th>
            <th>
                Apprive
            </th>
            <th>
                Decline
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.FanUser.Username %>
            </td>
            <td>
                <%= Html.ActionLink("Approve", "Approve", new { id = item.Id })%> |
            </td>
            <td>
                <%= Html.ActionLink("Decline", "Decline", new { id = item.Id })%> |
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

