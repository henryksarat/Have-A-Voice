<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.IssueReply>" %>

<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View a Reply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="col-24 m-top30">
	
    <% using (Html.BeginForm(new { issueReplyId = Model.Id })) { %>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <div class="clear">&nbsp;</div>
        <div class="m-btm10">
            <%= SharedContentStyleHelper.ProfilePictureDiv(Model.Issue.User, false, "col-3 center issue-profile", "profile")%>
            <%= IssueHelper.IssueInformationDiv(Model.Issue, "m-lft col-18 m-rgt comment", "col-17 p-v10 options", "push-10 col-2 center", "push-10 col-2 center", "push-10 col-3 center")%>
            <%= SharedContentStyleHelper.TimeStampDiv(Model.Issue.DateTimeStamp, "col-3", "p-a10", "date-tile", "MMM", "dd")%>
            <div class="clear">&nbsp;</div>
        </div>
        <div class="clear">&nbsp;</div>
        <%= IssueReplyHelper.IssueReply(Model) %>
        <% foreach (IssueReplyComment myComment in Model.IssueReplyComments) { %>
            <%= IssueReplyCommentHelper.BuildComment(myComment)%>
        <% } %>

		<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
			<div class="reply">
				<div class="row">
					<div class="push-6 col-15 comment">
						You must be logged in to reply.
						<div class="clear">&nbsp;</div>
					</div>
				</div>
			</div>
		<% } else { %>
			<div class="reply">
				<div class="row">
					<div class="push-6 col-3 center">
                        <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
						<img src="<%= myUserInfo.ProfilePictureUrl %>" alt="<%= myUserInfo.FullName %>" class="profile" />
					</div>
					<div class="push-6 m-lft col-12 m-rgt comment">
						<span class="speak-lft">&nbsp;</span>
						<div class="p-a10">
							<%= Html.ValidationMessage("Comment", "*") %>
							<%= Html.TextArea("Comment", string.Empty, 5, 50, new { resize = "none", style = "width:100%" }) %>
							<div class="clear">&nbsp;</div>
							<hr />
							<div class="col-11">
								<input type="submit" value="Submit" class="button" />
								<input type="button" value="Clear" class="button" />
								<div class="clear">&nbsp;</div>
							</div>
							<div class="clear">&nbsp;</div>
						</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
		<% } %>
    <% } %>

    </div>
</asp:Content>
