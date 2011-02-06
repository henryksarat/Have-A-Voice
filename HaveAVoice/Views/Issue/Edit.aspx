<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditIssueReply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("ISSUES", "Index", null, new { @class = "issue-create" }) %>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
	    	<%= Html.ActionLink("CREATE NEW", "Create", null, new { @class = "issue-create" }) %>
	    	<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">EDIT ISSUE</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 b-wht">
    		<div class="spacer-30">&nbsp;</div>
    		
    		<div class="push-1 col-22">

			    <%= Html.ValidationSummary("Your issue wasn't edited. Please correct the errors and try again.") %>
			    <%= Html.Encode(ViewData["Message"])%>
				<div class="clear">&nbsp;</div>
				
			    <% using (Html.BeginForm("Edit", "Issue", FormMethod.Post, new { @class = "create" })) { %>
	                <div class="col-4 m-rgt right">
	                	<label for="Title">Title:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("Title", Model.Title) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12">
	                	<%= Html.ValidationMessage("Title", "*") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>
	                
	                <div class="col-4 m-rgt right">
	                	<label for="Description">Description:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextArea("Description", Model.Description, new { cols = "40", rows = "4", resize = "none" }) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12">
	                	<%= Html.ValidationMessage("Description", "*") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

					<div class="col-10 right">
						<input type="submit" value="Submit" class="create" />
						<%= Html.ActionLink("Cancel", "Index", "", new { @class = "cancel" }) %>
						<div class="clear">&nbsp;</div>
					</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
				<div class="spacer-30">&nbsp;</div>
    		</div>
    	</div>
    </div>
</asp:Content>
