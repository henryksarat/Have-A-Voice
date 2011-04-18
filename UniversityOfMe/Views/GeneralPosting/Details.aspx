<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.GeneralPosting>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>


    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    Title: <%= Model.Title %><br />
    Body: <%= Model.Body %><br />
    Date Time Stamp: <%= Model.DateTimeStamp %><br />

    Replies
    <table>
    <% foreach (GeneralPostingReply myReply in Model.GeneralPostingReplies.OrderByDescending(gpr => gpr.Id)) { %>
        <tr>
            User: <td><%= NameHelper.FullName(myReply.User)%></td>
            Reply: <td><%= myReply.Reply %></td>
            Date Time Stamp: <td><%= myReply.DateTimeStamp %></td>
        </tr>
    <% } %>
    </table>

    <% using (Html.BeginForm("Create", "GeneralPostingReply")) {%>
        <%= Html.Hidden("GeneralPostingId", Model.Id)%>
        <%= Html.ValidationMessage("Reply", "*")%>
        <%= Html.TextArea("Reply")%>

        <p>
            <input type="submit" value="Create" />
        </p>
    <% } %>

</asp:Content>
