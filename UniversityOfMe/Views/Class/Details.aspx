<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Class>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

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

    Class Replies
    <table>
    <% foreach (var item in Model.ClassReplies.OrderByDescending(b => b.Id)) { %>
        <tr>
            <td><%= NameHelper.FullName(item.User) %></td>
            <td><%= item.Reply %></td>
            <td><%= item.DateTimeStamp %></td>
        </tr>
    <% } %>
    </table>

    <% using (Html.BeginForm("Create", "ClassReply")) {%>
        <%= Html.Hidden("ClassId", Model.Id) %>
        <%= Html.ValidationMessage("Reply", "*")%>
        <%= Html.TextArea("Reply")%>

        <p>
            <input type="submit" value="Create" />
        </p>
    <% } %>

</asp:Content>
