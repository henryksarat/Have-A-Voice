<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<ClassEnrollment>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <table>
        <tr>    
            <td>
                <% foreach (ClassEnrollment myClass in Model.Get()) { %>
                    <%= NameHelper.FullName(myClass.User)%>
                <% } %>
            </td>
        </tr>
    </table>
</asp:Content>
