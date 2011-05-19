<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Event>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>


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
                UserId
            </th>
            <th>
                DateStart
            </th>
            <th>
                DateEnd
            </th>
            <th>
                Information
            </th>
            <th>
                EntireSchool
            </th>
            <th>
                Deleted
            </th>
            <th>
                DeletedUserId
            </th>
        </tr>

    <% foreach (var item in Model.Get()) { %>
    
        <tr>
            <td>
                <%: item.Id %>
            </td>
            <td>
                <%: item.UniversityId %>
            </td>
            <td>
                <%: item.UserId %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.StartDate)%>
            </td>
            <td>
                <%: String.Format("{0:g}", item.EndDate) %>
            </td>
            <td>
                <%: item.Information %>
            </td>
            <td>
                <%: item.EntireSchool %>
            </td>
            <td>
                <%: item.Deleted %>
            </td>
            <td>
                <%: item.DeletedUserId %>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

