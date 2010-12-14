<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.View.UserDetailsModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>UserList</h2>
    
    <%= Html.Encode(ViewData["Message"]) %>
    <%= Html.Encode(ViewData["ErrorMessage"]) %>
    <% if (Model != null) { %>
        <table>
            <tr>
                <th>
                    Username
                </th>
                <td></td>
                <td></td>
            </tr>

        <% foreach (var item in Model) { %>
        
            <tr>
                <td>
                    <%= Html.Encode(item.Username) %>
                </td>
                <td>
                    <%= UserHelper.MessageImage(item.UserId, item.Username, item.CanMessage)%>
                </td>
                <td>
                    <%= UserHelper.ListenImage(item.UserId, item.Username, item.CanListen)%>
                </td>
            </tr>
        <% } %>
    <% } %>

    </table>
</asp:Content>

