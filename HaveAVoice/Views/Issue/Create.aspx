<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("ISSUES", "Index", null, new { @class = "issue-create" }) %>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CREATE</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 b-wht">
    		<div class="spacer-30">&nbsp;</div>
    		
    		<div class="push-1 col-22">

			    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
				<div class="clear">&nbsp;</div>
			    <% using (Html.BeginForm("Create", "Issue", FormMethod.Post, new { @class = "create" })) { %>
	                <%= Html.Hidden("City", HAVUserInformationFactory.GetUserInformation().Details.City) %>
	                <%= Html.Hidden("State", HAVUserInformationFactory.GetUserInformation().Details.State) %>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">Title:</label>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("Title") %>
	                </div>
	                <div class="col-12">
	                	<%= Html.ValidationMessage("Title", "*") %>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>
	                
	                <div class="col-4 m-rgt right">
	                	<label for="Description">Description:</label>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextArea("Description", null, new { cols = "40", rows = "4", resize = "none" }) %>
	                </div>
	                <div class="col-12">
	                	<%= Html.ValidationMessage("Description", "*") %>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

					<div class="col-10 right">
						<input type="submit" value="Create" class="create" />
						<%= Html.ActionLink("Cancel", "Index", "", new { @class = "cancel" }) %>
					</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
				<div class="spacer-30">&nbsp;</div>
    		</div>
    	</div>
    </div>
</asp:Content>

