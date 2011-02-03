﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.IssueReplyWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditIssueReply
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
		            <% /* UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); */ %>
					<img src="<% /* = myUserInfo.ProfilePictureUrl */ %>" alt="<% /* = myUserInfo.Details.Username */ %>" class="profile" />
				</div>
				<div class="m-lft col-14 comment">
					<span class="speak-lft">&nbsp;</span>
					<div class="p-a10">
						<%= Html.ValidationMessage("Body", "*") %>
						<%= Html.TextArea("Body", Model.Body, 5, 63, new { resize = "none", @class = "comment" }) %>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
							<%= Html.ValidationMessage("Disposition", "*")%>
							<div class="clear">&nbsp;</div>
							<label for="Like">Like</label>
							<%= Html.RadioButton("Disposition", (int)Disposition.Like, Model.Disposition == (int)Disposition.Like ? true : false) %>
							<label for="Dislike">Dislike</label>
							<%= Html.RadioButton("Disposition", (int)Disposition.Dislike, Model.Disposition == (int)Disposition.Dislike ? true : false) %>
						</div>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
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
</asp:Content>
