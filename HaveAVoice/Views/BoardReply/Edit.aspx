<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.BoardReplyWrapper>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Board Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
	<% Html.RenderPartial("Validation"); %>
	<div class="clear">&nbsp;</div>
	
	<% using (Html.BeginForm()) { %>
		<div class="reply m-top30">
			<div class="row">
				<div class="col-2 center">
		            <% UserInformationModel<User> myUserInfo = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation(); %>
					<img src="<%= myUserInfo.ProfilePictureUrl %>" alt="<%= myUserInfo.FullName  %>" class="profile" />
					<div class="clear">&nbsp;</div>
				</div>
				<div class="m-lft col-14 comment">
					<span class="speak-lft">&nbsp;</span>
					<div class="p-a10">
						<%= Html.TextArea("Message", Model.Message, 5, 63, new { resize = "none", @class = "comment" }) %>
						<span class="req">
							<%= Html.ValidationMessage("Message", "*") %>
						</span>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
							<input type="submit" value="Post" class="button" />
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
