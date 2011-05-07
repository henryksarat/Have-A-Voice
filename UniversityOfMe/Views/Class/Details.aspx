<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Class>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>


    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    Class Code: <%= Model.ClassCode %><br />
    Class Title: <%= Model.ClassTitle %><br />
    Academic Term: <%= Model.AcademicTerm.DisplayName %><br />
    Year: <%= Model.Year %><br />
    Details: <%= Model.Details %><br />
    Enrolled Students: <%= Model.ClassEnrollments.Count %>

    <% if(ClassHelper.IsEnrolled(UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(), Model)) { %>
        <% using (Html.BeginForm("Delete", "ClassEnrollment")) {%>
                    <%= Html.Hidden("ClassId", Model.Id) %>

                    <p>
                        <input type="submit" value="Remove" />
                    </p>
        <% } %>
    <% } else { %>
        <% using (Html.BeginForm("Create", "ClassEnrollment")) {%>
                    <%= Html.Hidden("ClassId", Model.Id) %>

                    <p>
                        <input type="submit" value="Enroll" />
                    </p>
        <% } %>
    <% } %> <br /><br />

    Currently enrolled:<br />
    <% foreach(ClassEnrollment myEnrollment in Model.ClassEnrollments) { %>
        <%= NameHelper.FullName(myEnrollment.User) %><br />
    <% } %><br /><br />

    <table>
        <tr>
            <td>
                <b>Class Replies</b><br /><br />
                <% foreach (var item in Model.ClassBoards.OrderByDescending(b => b.Id)) { %>
                    <%= NameHelper.FullName(item.User) %><br />
                    <%= item.Reply %><br />
                    <%= item.DateTimeStamp %><br />
                    -----------------<br /><br />
                <% } %>
            </td>
            <td>
                <b>Class Reviews</b><br /><br />
                <% foreach (var item in Model.ClassReviews.OrderByDescending(b => b.Id)) { %>
                    <% if (item.Anonymous) { %>
                        Anonymous    
                    <% } else { %>
                        <%= NameHelper.FullName(item.User) %>
                    <% } %><br />
                    <%= item.Rating %><br />
                    <%= item.Review %><br />
                    -----------------<br />
                <% } %>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td>
                Post to board: <br />
                <% using (Html.BeginForm("Create", "ClassBoard")) {%>
                    <%= Html.Hidden("ClassId", Model.Id) %>
                    <%= Html.ValidationMessage("BoardMessage", "*")%>
                    <%= Html.TextArea("BoardMessage")%>

                    <p>
                        <input type="submit" value="Post" />
                    </p>
                <% } %>
            </td>
            <td>
                Post to board: <br />
                <% using (Html.BeginForm("Create", "ClassReview")) {%>
                    <%= Html.Hidden("ClassId", Model.Id) %>
                    Review:<br />
                    <%= Html.ValidationMessage("Review", "*")%>
                    <%= Html.TextArea("Review")%><br /><br />
                    
                    Rating:
                    <%= Html.ValidationMessage("Rating", "*")%>
                    <%= Html.TextBox("Rating")%>
                    Anonymous:
                    <%= Html.CheckBox("Anonymous")%><br /><br />

                    <p>
                        <input type="submit" value="Post" />
                    </p>
                <% } %> 
            </td>
        </tr>
    </table>
</asp:Content>
