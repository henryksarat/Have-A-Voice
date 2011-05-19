<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Club>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>


    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% bool myIsAdmin = ClubHelper.IsAdmin(UserInformationFactory.GetUserInformation().Details, Model.Get().Id); %>
    Club Members
    <table>
    <% foreach (var item in Model.Get().ClubMembers.Where(cm => cm.Deleted == false)) { %>
        <tr>
            <td><%= NameHelper.FullName(item.ClubMemberUser) %></td>
            <td>
                <% if (myIsAdmin) { %>
                    <%= Html.ActionLink("Remove", "Remove", "ClubMember", new { userId = item.ClubMemberUser.Id, clubId = Model.Get().Id }, null)%>
                <% } %>
            </td>
        </tr>
    <% } %>
    </table>

    Club Board
    <table>
    <% foreach (var item in Model.Get().ClubBoards.OrderByDescending(b => b.DateTimeStamp)) { %>
        <tr>
            <td><%= NameHelper.FullName(item.User) %></td>
            <td><%= item.Message %></td>
            <td><%= item.DateTimeStamp %></td>
        </tr>
    <% } %>
    </table>

    <% using (Html.BeginForm("Create", "ClubBoard")) {%>
        <%= Html.Hidden("ClubId", Model.Get().Id)%>
        <%= Html.ValidationMessage("BoardMessage", "*")%>
        <%= Html.TextArea("BoardMessage")%>

        <p>
            <input type="submit" value="Create" />
        </p>
    <% } %>

</asp:Content>
