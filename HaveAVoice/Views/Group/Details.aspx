<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Group>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>

Group Name: <%= Model.Name %><br />
Group Description: <%= Model.Description %><br />
Auto Accept: <%= Model.AutoAccept %><br />
<br />
Zip Code:<br />
<% foreach (GroupZipCodeTag myZipCode in Model.GroupZipCodeTags) { %>
    <%= myZipCode.ZipCode %><br />
<% } %><br /><br />

Keyword:<br />
<% foreach (GroupTag myTag in Model.GroupTags) { %>
    <%= myTag.Tag %><br />
<% } %><br /><br />

City State Tag:<br />
<% foreach (GroupCityStateTag myCityStateTag in Model.GroupCityStateTags) { %>
    <%= myCityStateTag.City %>, <%= myCityStateTag.State %><br />
<% } %><br /><br />

<% if(GroupHelper.IsAllowedToEdit(myUserInfo, Model)) { %>
    <a href="/Group/Edit/<%= Model.Id %>">Edit</a>
<% } %>


</asp:Content>