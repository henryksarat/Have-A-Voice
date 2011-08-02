<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<a class="issue-create" href="/Issue/List">ISSUES</a>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CREATE</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 b-wht">
    		<div class="spacer-30">&nbsp;</div>
    		
    		<div class="push-1 col-22">
                <% Html.RenderPartial("Message"); %>
			    <% Html.RenderPartial("Validation"); %>
				<div class="clear">&nbsp;</div>
				
			    <% using (Html.BeginForm("Create", "Issue", FormMethod.Post, new { @class = "create" })) { %>
	                <%= Html.Hidden("City", HAVUserInformationFactory.GetUserInformation().Details.City) %>
	                <%= Html.Hidden("State", HAVUserInformationFactory.GetUserInformation().Details.State) %>
                    <%= Html.Hidden("ZipCode", HAVUserInformationFactory.GetUserInformation().Details.Zip) %>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">Title:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("Title") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("Title", "*") %>
		                </span>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>
	                
	                <div class="col-4 m-rgt right">
	                	<label for="Description">Description:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextArea("Description", null, new { cols = "80", rows = "8", resize = "none" }) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("Description", "*") %>
		                </span>
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

					<div class="col-10 right">
						<input type="submit" value="Create" class="create" />
						<%= Html.ActionLink("Cancel", "List", "Issue", null, new { @class = "cancel" }) %>
						<div class="clear">&nbsp;</div>
					</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
				<div class="spacer-30">&nbsp;</div>
    		</div>
    	</div>
    </div>
</asp:Content>

