﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Feedback>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("Message"); %>

    <h2>View</h2>
    <table>
        <tr>
            <th></th>
            <th>
                DateTimeStamp
            </th>
            <th>
                User
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
                <%: HaveAVoice.Helpers.NameHelper.FullName(item.User) %>
            </td>
            <td>
                <%: item.Text %>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

