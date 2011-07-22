<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditGroupModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>

<div class="col-24">
    <div class="spacer-30">&nbsp;</div>
    <div class="clear">&nbsp;</div>
    
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%=Html.ActionLink("GROUPS", "List", null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold"><%= Model.ViewAction.ToString().ToUpper() %></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="col-24 b-wht">
    	<div class="spacer-30">&nbsp;</div>
    		
    	<div class="push-1 col-22">
            <% Html.RenderPartial("Message"); %>
			<% Html.RenderPartial("Validation"); %>
			<div class="clear">&nbsp;</div>
				
			<% using (Html.BeginForm(Model.ViewAction.ToString(), "Group", FormMethod.Post, new { @class = "create" })) { %>
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
	                <%= Html.RadioButton("AutoAccept", true, Model.AutoAccept)%> Yes
                    <%= Html.RadioButton("AutoAccept", false, !Model.AutoAccept)%> No
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

                <div class="col-4 m-rgt right">
	                <label for="Name">Make Public:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-6">
	                <%= Html.RadioButton("MakePublic", true, Model.MakePublic)%> Yes
                    <%= Html.RadioButton("MakePublic", false, !Model.MakePublic)%> No
	                <div class="clear">&nbsp;</div>
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
	                <label for="Description">Zip Code Tags:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-17">
	                <%= Html.TextArea("ZipCodeTags", null, new { cols = "80", rows = "3", resize = "none" }) %>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1">
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
	            <div class="col-17">
	                <%= Html.TextArea("KeywordTags", null, new { cols = "80", rows = "3", resize = "none" }) %>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1">
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
		                <%= Html.ValidationMessage("CityTag", "*") %>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

                <div class="col-4 m-rgt right">
	                <label for="Name">State Tag:</label>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-2">
	                <%= Html.DropDownList("StateTag", Model.States)%>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1">
	                <span class="req">
		                <%= Html.ValidationMessage("StateTag", "*")%>
		            </span>
	            </div>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-10">&nbsp;</div>

				<div class="col-10 right">
					<input type="submit" value="Submit" class="create" />
                    <% if (Model.ViewAction == Social.Generic.Helpers.ViewAction.Create) { %>
					    <%= Html.ActionLink("Cancel", "List", "Group", null, new { @class = "cancel" })%>
                    <% } else { %>
                        <%= Html.ActionLink("Cancel", "Details", "Group", new { id = Model.Id }, new { @class = "cancel" })%>
                    <% } %>
					<div class="clear">&nbsp;</div>
				</div>
			<% } %>
			<div class="clear">&nbsp;</div>
			<div class="spacer-30">&nbsp;</div>
    	</div>
    </div>
</div>