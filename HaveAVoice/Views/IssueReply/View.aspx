<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueReplyDetailsModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View a Reply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="col-24">
	
    <% using (Html.BeginForm()) { %>
        <%= Html.ValidationSummary("Your comment wasn't posted. Please correct the errors and try again.") %>
        <div class="clear">&nbsp;</div>

        <%= Html.Encode(ViewData["Message"]) %>
        <%= Html.Encode(TempData["Message"]) %>
        <div class="clear">&nbsp;</div>

        <%= IssueHelper.IssueReply(Model) %>
        <% foreach (IssueReplyComment comment in Model.Comments) { %>
            <%= IssueHelper.Comment(comment) %>
            <% if (comment.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id || HAVPermissionHelper.AllowedToPerformAction(HAVUserInformationFactory.GetUserInformation(), HAVPermission.Edit_Any_Issue_Reply_Comment)) { %>
	            <%= Html.ActionLink("Edit", "Edit", "IssueReplyComment", new { id = comment.Id }, null)%>
            <% } %>
            <% if (comment.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id || HAVPermissionHelper.AllowedToPerformAction(HAVUserInformationFactory.GetUserInformation(), HAVPermission.Delete_Any_Issue_Reply_Comment)) { %>
                <%= Html.ActionLink("Delete", "Delete", "IssueReplyComment", new { id = comment.Id, replyId = Model.IssueReply.Id }, null)%>
            <% } %>
        <% } %>

		<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
			<div class="reply">
				<div class="row">
					<div class="push-6 col-15 comment">
						You must be logged in to reply.
					</div>
				</div>
			</div>
		<% } else { %>
			<div class="reply">
				<div class="row">
					<div class="push-6 col-3 center">
						<img src="http://www2.pictures.zimbio.com/img/5519/Alicia/5986c.nZBops.jpg" alt="Gerard Butler" class="profile" />
					</div>
					<div class="push-6 m-lft col-12 m-rgt comment">
						<span class="speak-lft">&nbsp;</span>
						<div class="p-a10">
							<%= Html.ValidationMessage("Body", "*") %>
							<%= Html.TextArea("Comment", Model.Comment, 5, 63, new { resize = "none", @class = "comment" }) %>
							<div class="clear">&nbsp;</div>
							<hr />
							<div class="col-11">
								<input type="submit" value="Submit" class="button" />
								<input type="button" value="Clear" class="button" />
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