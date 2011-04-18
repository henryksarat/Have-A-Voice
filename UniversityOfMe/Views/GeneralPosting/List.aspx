﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<UniversityOfMe.Models.GeneralPosting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>


    <table>
        <tr>
            <th>
                Full Name
            </th>
            <th>
                Title
            </th>
            <th>
                Body
            </th>
            <th>
                Date Time Stamp
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= UniversityOfMe.Helpers.NameHelper.FullName(item.User) %>
            </td>
            <td>
                <%= item.Title %>
            </td>
            <td>
                <%= item.Body %>
            </td>
            <td>
                <%= item.DateTimeStamp %>
            </td>
        </tr>
    
    <% } %>
    </table>

</asp:Content>

