<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditGroupModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>



    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("GROUPS", "List", null, new { @class = "issue-create" }) %>
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
				
			    <% using (Html.BeginForm("Create", "Group", FormMethod.Post, new { @class = "create" })) { %>
	                <div class="col-4 m-rgt right">
	                	<label for="Name">Name:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("Name") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("Name", "*") %>
		                </span>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

                    <div class="col-4 m-rgt right">
	                	<label for="Name">Your Title:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("CreatorTitle", GroupConstants.DEFAULT_GROUP_LEADER_TITLE) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("CreatorTitle", "*")%>
		                </span>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

                    <div class="col-4 m-rgt right">
	                	<label for="Name">Auto Accept:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                    <%= Html.RadioButton("AutoAccept", true)%> Yes
                        <%= Html.RadioButton("AutoAccept", false, true)%> No
	                	<div class="clear">&nbsp;</div>
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

                    <div class="col-4 m-rgt right">
	                	<label for="Description">Zip Code Tags:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextArea("ZipCodeTags", null, new { cols = "80", rows = "3", resize = "none" }) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("ZipCodeTags", "*")%>
		                </span>
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

                    <div class="col-4 m-rgt right">
	                	<label for="Description">Keyword Tags:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextArea("KeywordTags", null, new { cols = "80", rows = "3", resize = "none" }) %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("KeywordTags", "*")%>
		                </span>
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

                    <div class="col-4 m-rgt right">
	                	<label for="Name">City Tag:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("CityTag") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("NameTag", "*") %>
		                </span>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

                    <div class="col-4 m-rgt right">
	                	<label for="Name">State Tag:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.DropDownList("StateTag", Model.States)%>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-12 m-lft">
	                	<span class="req">
		                	<%= Html.ValidationMessage("StateTag", "*")%>
		                </span>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>


					<div class="col-10 right">
						<input type="submit" value="Create" class="create" />
						<%= Html.ActionLink("Cancel", "List", "Group", null, new { @class = "cancel" }) %>
						<div class="clear">&nbsp;</div>
					</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
				<div class="spacer-30">&nbsp;</div>
    		</div>
    	</div>
    </div>
</asp:Content>