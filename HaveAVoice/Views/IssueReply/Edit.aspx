<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.IssueReplyWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Issue Reply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>

	<%= Html.ValidationSummary("<div class='error'>Your reply wasn't posted. Please correct your errors and try again.</div>") %>
	<%= Html.Encode(ViewData["Message"]) %>
	<div class="clear">&nbsp;</div>
	
	<% using (Html.BeginForm()) { %>
		<div class="reply m-top30">
			<div class="row">
				<div class="col-2 center">
		            <% UserInformationModel myUserInfo = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation(); %>
					<img src="<%= myUserInfo.ProfilePictureUrl %>" alt="<%= myUserInfo.FullName  %>" class="profile" />
					<div class="clear">&nbsp;</div>
				</div>
				<div class="m-lft col-14 comment">
					<span class="speak-lft">&nbsp;</span>
					<div class="p-a10">
						<%= Html.TextArea("Body", Model.Body, 5, 63, new { resize = "none", @class = "comment" }) %>
						<span class="req">
							<%= Html.ValidationMessage("Body", "*") %>
						</span>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
							<label for="Like">Like</label>
							<%= Html.RadioButton("Disposition", (int)Disposition.Like, Model.Disposition == (int)Disposition.Like ? true : false) %>
							<label for="Dislike">Dislike</label>
							<%= Html.RadioButton("Disposition", (int)Disposition.Dislike, Model.Disposition == (int)Disposition.Dislike ? true : false) %>
							<span class="req">
								<%= Html.ValidationMessage("Disposition", "*") %>
							</span>
							<div class="clear">&nbsp;</div>
						</div>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
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
</asp:Content>
