<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Professor>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Validation"); %><br />
    <% Html.RenderPartial("Message"); %>
        

    <table>
        <tr>
            <th>
            </th>
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
                <img src="<%= PhotoHelper.ConstructProfessorUrl(Model.Get()) %>" />
            </td>
            <td>
                <%: Model.Get().UniversityId %>
            </td>
            <td>
                <%: Model.Get().FirstName%>
            </td>
            <td>
                <%: Model.Get().LastName%>
            </td>
        </tr>
    </table>

    Suggest a photo:<br />
    <% using (Html.BeginForm("SuggestProfessorPicture", "Professor", FormMethod.Post, new { enctype = "multipart/form-data" })) {%>
        <%= Html.Hidden("ProfessorId", Model.Get().Id)%>
        <%= Html.Hidden("FirstName", Model.Get().FirstName)%>
        <%= Html.Hidden("LastName", Model.Get().LastName)%>
        
        <div class="editor-label">
            <%: Html.Label("Optional Professor Image") %>
        </div>
        <div class="editor-field">
            <input type="file" id="ProfessorImage" name="ProfessorImage" size="23" />
            <%: Html.ValidationMessage("ProfessorImage", "*")%>
        </div>

        <p>
            <input type="submit" value="Suggest" />
        </p>
    <% } %>
    

    <br /><br />
    <a href="/<%= Model.Get().UniversityId %>/ProfessorReview/Create/<%= Model.Get().Id %>">Create New Review</a><br /><br />

    <% foreach (ProfessorReview myReview in Model.Get().ProfessorReviews.OrderByDescending(pr => pr.DateTimeStamp)) {  %>
        By user: <%= myReview.Anonymous ? "Anonymous" : NameHelper.FullName(myReview.User) %><br />
        Date Posted: <%= myReview.DateTimeStamp %><br />
        Rating: <%= myReview.Rating %><br />          
        Term: <%= myReview.AcademicTerm.DisplayName %><br />
        Year: <%= myReview.Year %><br />
        Review: <%= myReview.Review %><br /><br />
    <% } %>
</asp:Content>

