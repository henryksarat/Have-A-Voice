<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Issue>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserResults
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Issue Results</h2>

    <% Html.RenderPartial("Message"); %>

    <% foreach (Issue myIssue in Model) { %>
        Title: <%= myIssue.Title %><br />
        Description: <%= myIssue.Description %><br />
        Date: <%= myIssue.DateTimeStamp %><br />
        Posted By FullName = <%= NameHelper.FullName(myIssue.User) %><br />
        Posted By Profile Picture: <%= PhotoHelper.ProfilePicture(myIssue.User)%><br />
        Posted By Short Url: <%= myIssue.User.ShortUrl %><br /><br />
    <% } %>

</asp:Content>

