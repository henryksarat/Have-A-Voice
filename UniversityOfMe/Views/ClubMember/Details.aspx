<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ClubMember>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Constants" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ClubMemberDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="club"><%= Model.Get().Club.Name %> New Member Approval</span> 
		</div> 
        <div class="create"> 
            <% using (Html.BeginForm("Verdict", "ClubMember", FormMethod.Post)) {%>
                <%= Html.Hidden("ClubMemberId", Model.Get().Id) %>
                <%= Html.Hidden("ClubId", Model.Get().ClubId) %>

                <div style="vertical-align:middle">
                    <a href="<%= URLHelper.ProfileUrl(Model.Get().ClubMemberUser) %>"><img src="<%= PhotoHelper.ProfilePicture(Model.Get().ClubMemberUser) %>" class="profile big mr22" /></a>
                    <a class="itemlinked" href="<%= URLHelper.ProfileUrl(Model.Get().ClubMemberUser) %>"><%= NameHelper.FullName(Model.Get().ClubMemberUser) %></a>
                </div>

			    <label for="Name">Member Title:</label> 
			    <%= Html.TextBox("Title", ClubConstants.DEFAULT_NEW_MEMBER_TITLE, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Title", "*")%>

			    <label for="Administrator">Administrator:</label> 
			    <%= Html.RadioButton("Administrator", true) %> Yes
                <%= Html.RadioButton("Administrator", false, true)%> No

			    <div class="right"> 
                    <button name="approved" class="btn blue mr14" value="true">Approve</button>
                    <button name="approved" class="btn blue" value="false">Deny</button>
    			</div> 
            <% } %>
        </div>
	</div> 
</asp:Content>
