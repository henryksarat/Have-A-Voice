<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Provide Us Feedback
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
	<%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>
	<div class="clear">&nbsp;</div>
	
	<% using (Html.BeginForm()) { %>
		<div class="reply m-top30">
			<div class="row">
				<div class="col-2 center">
		            <% UserInformationModel myUserInfo = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation(); %>
					<img src="<%= myUserInfo.ProfilePictureUrl %>" alt="<%= myUserInfo.FullName  %>" class="profile" />
				</div>
				<div class="m-lft col-14 comment">
					<span class="speak-lft">&nbsp;</span>
					<div class="p-a10">
						<%= Html.TextArea("Feedback", Model.Message, 5, 63, new { resize = "none" }) %>
						<span class="req">
							<%= Html.ValidationMessage("Feedback", "*") %>
						</span>
						<div class="clear">&nbsp;</div>
						<hr />
						<div class="col-13">
							<input type="submit" value="Send" class="button" />
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
