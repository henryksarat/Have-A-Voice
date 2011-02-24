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
	<div class="col-24">
	
    <% using (Html.BeginForm(new { issueReplyId = Model.Id })) { %>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <div class="clear">&nbsp;</div>

        <%= IssueHelper.IssueReply(Model) %>
        <% foreach (IssueReplyComment comment in Model.IssueReplyComments) { %>
            <%= IssueHelper.Comment(comment) %>
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
							<%= Html.TextArea("Comment", string.Empty, 5, 63, new { resize = "none", @class = "comment" }) %>
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
