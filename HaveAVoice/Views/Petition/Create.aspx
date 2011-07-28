<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CreatePetitionModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Petition
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

<div class="col-24">
    <div class="spacer-30">&nbsp;</div>
    <div class="clear">&nbsp;</div>
    
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%=Html.ActionLink("PETITIONS", "List", "Petition", null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold">CREATE PETITION</span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="col-24 b-wht">
    	<div class="spacer-30">&nbsp;</div>
    		
    	<div class="push-1 col-22">
            <% Html.RenderPartial("Message"); %>
			<% Html.RenderPartial("Validation"); %>
			<div class="clear">&nbsp;</div>
				
			<% using (Html.BeginForm("Create", "Petition", FormMethod.Post, new { @class = "create" })) { %>
	            <div class="col-4 m-rgt right">
	                <label for="Name">Title:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-6">
	                <%= Html.TextBox("Title") %>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-12 m-lft">
	                <span class="req">
		                <%= Html.ValidationMessage("Title", "*")%>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>
	                
	            <div class="col-4 m-rgt right">
    	            <label for="Description">Description:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-17">
	                <%= Html.TextArea("Description", null, new { cols = "80", rows = "8", resize = "none" }) %>

	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1">
	                <span class="req">
		                <%= Html.ValidationMessage("Description", "*") %>
		            </span>
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

                <div class="col-4 m-rgt right">
	                <label for="Name">Zip Code Origin:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-6">
	                <%= Html.TextBox("ZipCode", myUserInfo.Details.Zip)%>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-12 m-lft">
	                <span class="req">
		                <%= Html.ValidationMessage("ZipCode", "*")%>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

                <div class="col-4 m-rgt right">
	                <label for="Name">City Origin:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-6">
	                <%= Html.TextBox("City", myUserInfo.Details.City)%>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-12 m-lft">
	                <span class="req">
		                <%= Html.ValidationMessage("City", "*")%>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

                <div class="col-4 m-rgt right">
	                <label for="Name">State Origin:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-2">
	                <%= Html.DropDownList("State", Model.States)%>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1">
	                <span class="req">
		                <%= Html.ValidationMessage("State", "*")%>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

				<div class="col-10 right">
					<input type="submit" value="Submit" class="create" />
					<%= Html.ActionLink("Cancel", "List", "Petition", null, new { @class = "cancel" })%>
					<div class="clear">&nbsp;</div>
				</div>
			<% } %>
			<div class="clear">&nbsp;</div>
			<div class="spacer-30">&nbsp;</div>
    	</div>
    </div>
</div>
</asp:Content>