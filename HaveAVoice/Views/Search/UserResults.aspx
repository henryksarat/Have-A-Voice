<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.User>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserResults
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>User Results</h2>

    <% Html.RenderPartial("Message"); %>
    <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
    <% foreach (User myUser in Model) { %>
        Full Name: <%= NameHelper.FullName(myUser) %><br />
        Profile Picture: <%= PhotoHelper.ProfilePicture(myUser) %><br />
        Short Url: <%= myUser.ShortUrl %><br />
        <% if (myUserInfo!= null && !FriendHelper.IsFriend(myUserInfo.Details, myUser)) { %>
            <%= Html.ActionLink("Add Friend", "Add", "Friend", new { id = myUser.Id }, null)%><br />
        <% } %>
        <%= Html.ActionLink("Message", "Create", "Message", new { id = myUser.Id }, null) %><br />
        <br />
    <% } %>

</asp:Content>

