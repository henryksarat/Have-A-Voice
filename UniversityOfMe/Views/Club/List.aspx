<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Club>>" %>
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

    <% foreach (var item in Model.Get()) { %>
    
        <tr>
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

