<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateMessageModel<User>>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="Social.Messaging.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Send a Message
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
	    
	    <div class="col-3 center">
            <a class="name" href="<%= LinkHelper.Profile(Model.Get().SendToUser) %>">
                <%= NameHelper.FullName(Model.Get().SendToUser) %>
            </a>
            <div class="clear">&nbsp;</div>
            <a href="<%= LinkHelper.Profile(Model.Get().SendToUser) %>">
	    	    <img src="<%= PhotoHelper.ProfilePicture(Model.Get().SendToUser) %>" alt="<%= NameHelper.FullName(Model.Get().SendToUser) %>" class="profile" />
            </a>
	    	<div class="clear">&nbsp;</div>
	    </div>
	    <div class="col-18">	    	
	        <% using (Html.BeginForm("Create", "Message", FormMethod.Post, new { @class = "create" })) { %>
		        <%= Html.Hidden("ToUserId", Model.Get().SendToUser.Id)%>
		        
		        <div class="clear">&nbsp;</div>
		        
		        <div class="col-18 m-btm10">
			    	<div class="m-lft col-13 m-rgt">
                        <div class="col-2">
			    		    <label for="subject">Subject:</label>
                        </div>
                        <div class="col-7">
			    		    <%= Html.TextBox("Subject", Model.Model.DefaultSubject) %>
			    		    <div class="clear">&nbsp;</div>
                        </div>
			    	    <div class="col-1 center">
			    		    <span class="req">
				    		    <%= Html.ValidationMessage("Subject", "*") %>
			    		    </span>
			    	    </div>
                    </div>
			    	<div class="clear">&nbsp;</div>
		        </div>
				<div class="col-18 m-btm10">
					<div class="m-lft col-13 m-rgt">
                        <div class="col-2">
						    <label>Message:</label>
                        </div>
                        <div class="col-7">
						    <%= Html.TextArea("Body", null, new { cols = "60", rows = "8", resize = "none", @class="fnt-12" })%>
						    <div class="clear">&nbsp;</div>
                        </div>
					</div>
					<div class="col-1 center">
						<span class="req">
							<%= Html.ValidationMessage("Body", "*") %>
						</span>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="push-9">
				    <input type="submit" class="create" value="Send" />
				    <%= Html.ActionLink("Cancel", "Inbox", "Message", new { @class = "cancel" }) %>
                </div>
	    	<% } %>
		    <div class="clear">&nbsp;</div>
	    </div>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>