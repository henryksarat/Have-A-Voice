<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<GeneralPosting>>" %>
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

    Title: <%= Model.Get().Title %><br />
    Body: <%= Model.Get().Body %><br />
    Date Time Stamp: <%= Model.Get().DateTimeStamp%><br />

    Replies
    <table>
    <% foreach (GeneralPostingReply myReply in Model.Get().GeneralPostingReplies.OrderByDescending(gpr => gpr.Id)) { %>
        <tr>
            User: <td><%= NameHelper.FullName(myReply.User)%></td>
            Reply: <td><%= myReply.Reply %></td>
            Date Time Stamp: <td><%= myReply.DateTimeStamp %></td>
        </tr>
    <% } %>
    </table>

    <% using (Html.BeginForm("Create", "GeneralPostingReply")) {%>
        <%= Html.Hidden("GeneralPostingId", Model.Get().Id)%>
        <%= Html.ValidationMessage("Reply", "*")%>
        <%= Html.TextArea("Reply")%>

        <p>
            <input type="submit" value="Create" />
        </p>
    <% } %>

</asp:Content>
