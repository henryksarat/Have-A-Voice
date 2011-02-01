<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<MessageWrapper>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Send a Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <div class="col-21">	
        <% Html.RenderPartial("Message"); %>
	    
	    <%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>
	    <div class="clear">&nbsp;</div>
	    
	    <div class="col-3">
	    	<img src="<%=Model.Model.ToUserProfilePictureUrl %>" alt="<%= Model.Model.ToUserName %>" class="profile" />
	    </div>
	    <div class="col-18">
	    	<div class="col-18">
	    		<h4><%= Html.Encode("Message: " + Model.Model.ToUserName)%></h4>
				<div class="clear">&nbsp;</div>
	    	</div>
	    	<div class="clear">&nbsp;</div>
	    	
	        <% using (Html.BeginForm("Create", "Message", FormMethod.Post, new { @class = "create" })) { %>
		        <%= Html.Hidden("ToUserId", Model.Model.ToUserId)%>
		        <%= Html.Hidden("ToUserName", Model.Model.ToUserName)%>
		        <%= Html.Hidden("ToUserProfilePictureUrl", Model.Model.ToUserProfilePictureUrl)%>
		        
		        <%= Html.Encode(ViewData["Message"]) %>
		        <div class="clear">&nbsp;</div>
		        
		        <div class="col-18 m-btm10">
			    	<div class="m-lft col-2 m-rgt right">
			    		<label>Subject:</label>
			    	</div>
			    	<div class="col-6">
			    		<%= Html.TextBox("Subject", "")%>
			    	</div>
			    	<div class="m-lft col-8">
			    		<%= Html.ValidationMessage("Subject", "*") %>
			    	</div>
			    	<div class="clear">&nbsp;</div>
		        </div>
				<div class="col-18 m-btm10">
					<div class="m-lft col-2 m-rgt right">
						<label>Message:</label>
					</div>
					<div class="col-6">
						<%= Html.TextArea("Body", null, new { cols = "31", rows = "4", resize = "none" })%>
					</div>
					<div class="m-lft col-8">
						<%= Html.ValidationMessage("Body", "*") %>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				
				<input type="submit" class="create" value="Send" />
				<%= Html.ActionLink("Cancel", "Index", null, new { @class = "cancel" }) %>
	    	<% } %>
		    <div class="clear">&nbsp;</div>
	    </div>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>