<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Message>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewMessage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>
    
    <div class="col-21">	
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="clear">&nbsp;</div>

        <% using (Html.BeginForm("CreateReply", "Message")) { %>
			<%= Html.Hidden("Id", Model.Model.Id) %>

            <% if (Model != null) { %>
	            <%= MessageHelper.MessageItem(NameHelper.FullName(Model.Model.FromUser), PhotoHelper.ProfilePicture(Model.Model.FromUser), Model.Model.Subject, Model.Model.Body, Model.Model.DateTimeStamp)%>
	
		         <% foreach (var reply in Model.Model.Replys) { %>
		         	<div class="m-btm10">
		         		<%= MessageHelper.MessageItem(NameHelper.FullName(reply.User), PhotoHelper.ProfilePicture(reply.User), "", reply.Body, reply.DateTimeStamp) %>
		         	</div>
	            <% } %>
				<div class="clear">&nbsp;</div>

				<div class="col-2 m-rgt bold fnt-12 c-gray">
					Reply:
					<div class="clear">&nbsp;</div>
				</div>
				<div class="col-8 fnt-12">
					<%= Html.TextArea("Reply", new { cols = "40", rows = "3", resize = "no" }) %>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="col-11 m-lft">
					<span class="req">
						<%= Html.ValidationMessage("Reply", "*") %>
					</span>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="clear">&nbsp;</div>

	            <div class="push-8 col-2 right">
	            	<input type="submit" value="Submit" class="submit" />
	            	<div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	        <% } %>
	    <% } %>
    </div>
</asp:Content>
