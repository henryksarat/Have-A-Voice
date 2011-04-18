<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Professor>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

    <table>
        <tr>
            <th>
                University
            </th>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
        </tr>
        <tr>
            <td>
                <%: Model.UniversityId %>
            </td>
            <td>
                <%: Model.FirstName%>
            </td>
            <td>
                <%: Model.LastName%>
            </td>
        </tr>
    </table>

    <br /><br />
    <a href="/<%= Model.UniversityId %>/ProfessorReview/Create/<%= Model.Id %>">Create New Review</a><br /><br />

    <% foreach(ProfessorReview myReview in Model.ProfessorReviews.OrderByDescending(pr => pr.DateTimeStamp)) {  %>
        By user: <%= myReview.Anonymous ? "Anonymous" : NameHelper.FullName(myReview.User) %><br />
        Date Posted: <%= myReview.DateTimeStamp %><br />
        Rating: <%= myReview.Rating %><br />          
        Term: <%= myReview.AcademicTerm.DisplayName %><br />
        Year: <%= myReview.Year %><br />
        Review: <%= myReview.Review %><br /><br />
    <% } %>
</asp:Content>

