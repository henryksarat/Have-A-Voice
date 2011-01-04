<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModelWithItemWrapped<ViewMessageModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

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
	            <%= MessageHelper.MessageItem(Model.Model.Message.FromUser.Username, Model.Model.Message.Subject, Model.Model.Message.Body, Model.Model.Message.DateTimeStamp)%>
	
		         <% foreach (var reply in Model.Model.Message.Replys) { %>
	                <%= MessageHelper.MessageItem(reply.User.Username, "", reply.Body, reply.DateTimeStamp)%>
	            <% } %>
		                    		
	            <%= Html.ValidationSummary("Reply was unsuccessful. Please correct the errors and try again.") %>
	            <%= Html.TextArea("Reply")%>
			    <%= Html.ValidationMessage("Reply", "*")%>
	            <input type="submit" value="Submit" />

	        <% } %>
	    <% } %>
    </div>
</asp:Content>
