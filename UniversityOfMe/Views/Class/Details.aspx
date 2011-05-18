<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Class>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>


    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    Class Code: <%= Model.Get().ClassCode %><br />
    Class Title: <%= Model.Get().ClassTitle%><br />
    Academic Term: <%= Model.Get().AcademicTerm.DisplayName%><br />
    Year: <%= Model.Get().Year%><br />
    Details: <%= Model.Get().Details%><br />
    Enrolled Students: <%= Model.Get().ClassEnrollments.Count%>

    <% if(ClassHelper.IsEnrolled(UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(), Model.Get())) { %>
        <% using (Html.BeginForm("Delete", "ClassEnrollment")) {%>
                    <%= Html.Hidden("ClassId", Model.Get().Id)%>

                    <p>
                        <input type="submit" value="Remove" />
                    </p>
        <% } %>
    <% } else { %>
        <% using (Html.BeginForm("Create", "ClassEnrollment")) {%>
                    <%= Html.Hidden("ClassId", Model.Get().Id)%>

                    <p>
                        <input type="submit" value="Enroll" />
                    </p>
        <% } %>
    <% } %> <br /><br />

    Currently enrolled:<br />
    <% foreach (ClassEnrollment myEnrollment in Model.Get().ClassEnrollments) { %>
        <%= NameHelper.FullName(myEnrollment.User) %><br />
    <% } %><br /><br />

    <%= Html.ActionLink("View all members", "ViewAllMembers", "Class", new { id = Model.Get().Id }, null) %><br />

    <table>
        <tr>
            <td>
                <b>Class Replies</b><br /><br />
                <% foreach (var item in Model.Get().ClassBoards.OrderByDescending(b => b.Id)) { %>
                    <%= NameHelper.FullName(item.User) %><br />
                    <%= item.Reply %><br />
                    <%= item.DateTimeStamp %><br />
                    -----------------<br /><br />
                <% } %>
            </td>
            <td>
                <b>Class Reviews</b><br /><br />
                <% foreach (var item in Model.Get().ClassReviews.OrderByDescending(b => b.Id)) { %>
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
                    <%= Html.Hidden("ClassId", Model.Get().Id) %>
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
                    <%= Html.Hidden("ClassId", Model.Get().Id)%>
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
