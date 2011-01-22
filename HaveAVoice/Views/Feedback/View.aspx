<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Feedback>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>View</h2>

    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>

    <table>
        <tr>
            <th></th>
            <th>
                DateTimeStamp
            </th>
            <th>
                Username
            </th>
            <th>
                Text
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: String.Format("{0:g}", item.DateTimeStamp) %>
            </td>
            <td>
                <%: item.User.Username %>
            </td>
            <td>
                <%: item.Text %>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

