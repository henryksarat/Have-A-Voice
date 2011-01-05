<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<ViewMessageModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewMessage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.UserModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">	
	    <% using (Html.BeginForm("CreateReply", "Message")) { %>
			<%= Html.Hidden("Id", Model.Model.Message.Id) %>
			<%= Html.Encode(ViewData["Message"]) %>
			<div class="clear">&nbsp;</div>

            <% if (Model != null) { %>
	            <%= MessageHelper.MessageItem(Model.Model.Message.FromUser.Username, ProfilePictureHelper.ProfilePicture(Model.Model.Message.FromUser), Model.Model.Message.Subject, Model.Model.Message.Body, Model.Model.Message.DateTimeStamp)%>
	
		         <% foreach (var reply in Model.Model.Message.Replys) { %>
	                <%= MessageHelper.MessageItem(reply.User.Username, ProfilePictureHelper.ProfilePicture(reply.User), "", reply.Body, reply.DateTimeStamp)%>
	            <% } %>
				<div class="clear">&nbsp;</div>

				<div class="col-2 m-rgt bold fnt-12 c-gray">
					Reply:
				</div>
				<div class="col-8 fnt-12">
					<%= Html.TextArea("Reply", null, new { cols = "40", rows = "3", resize = "no" })%>
				</div>
				<div class="col-11 m-lft">
					<%= Html.ValidationSummary("Reply was unsuccessful. Please correct the errors and try again.") %>
				    <%= Html.ValidationMessage("Reply", "*")%>
				</div>
				<div class="clear">&nbsp;</div>

	            <div class="push-8 col-2 right">
	            	<input type="submit" value="Submit" class="submit" />
	            </div>
	            <div class="clear">&nbsp;</div>
	        <% } %>
	    <% } %>
    </div>
</asp:Content>
