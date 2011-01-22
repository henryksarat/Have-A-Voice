<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.ErrorLog>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Error Log</h2>

    <table>
        <tr>
            <th>
                Exception
            </th>
            <th>
                InnerException
            </th>
            <th>
                StackTrace
            </th>
            <th>
                DateTimeStamp
            </th>
            <th>
                Details
            </th>
            <th>
                User
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Exception) %>
            </td>
            <td>
                <%= Html.Encode(item.InnerException) %>
            </td>
            <td>
                <%= Html.Encode(item.StackTrace) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.DateTimeStamp)) %>
            </td>
            <td>
                <%= Html.Encode(item.Details) %>
            </td>
            <td>
                <%= Html.Encode(item.User.Username) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

