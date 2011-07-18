<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupMember>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
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

Name: <%= NameHelper.FullName(Model.MemberUser) %>
<% using (Html.BeginForm("Verdict", "GroupMember", FormMethod.Post)) {%>
    <% if(Model.Approved == HAVConstants.PENDING) { %>
        <%= Html.Hidden("GroupMemberId", Model.Id) %>
        <%= Html.Hidden("GroupId", Model.GroupId) %>

        <div>
	        <label for="Name">Title for user:</label> 
	        <%= Html.TextBox("Title", GroupConstants.DEFAULT_NEW_MEMBER_TITLE)%>
            <%= Html.ValidationMessage("Title", "*")%>
        </div> 
        <div>
            <label for="Administrator">Admin Rights:</label> 
            <%= Html.RadioButton("Administrator", true)%> Yes
            <%= Html.RadioButton("Administrator", false, true)%> No
        </div>
		<div> 
            <button name="approved" value="true">Approve</button>
            <button name="approved" value="false">Deny</button>
    	</div> 
    <% } else if(Model.Approved == HAVConstants.APPROVED) { %>
        <a href="/Group/Remove/<%= Model.Id %>">Remove member</a>
    <% } %>
<% } %>

</asp:Content>