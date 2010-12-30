<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<%= Html.Encode(ViewData["Message"]) %>
    	<%= Html.Encode(TempData["Message"]) %>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("ISSUES", "Index", null, new { @class = "issue-create" }) %>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CREATE</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 b-wht">
    		<div class="spacer-30">&nbsp;</div>
		    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

		    <% using (Html.BeginForm()) {%>
		        <fieldset>
		            <legend>Fields</legend>
		            <p>
		                <%= Html.Encode(ViewData["Message"]) %>
		            </p>
		            <p>
		                <label for="Title">Title:</label>
		                <%= Html.TextBox("Title") %>
		                <%= Html.ValidationMessage("Title", "*") %>
		            </p>
		            <p>
		                <label for="Description">Description:</label>
		                <%= Html.TextArea("Description") %>
		                <%= Html.ValidationMessage("Description", "*") %>
		            </p>
		            <p>
		                <input type="submit" value="Create" />
		            </p>
		        </fieldset>		
		    <% } %>
    	</div>
    </div>
</asp:Content>

