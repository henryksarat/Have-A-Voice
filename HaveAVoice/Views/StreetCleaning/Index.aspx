<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Wrappers.IssueWrapper>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Region Check
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<a class="issue-create" href="http://www.haveavoice.com">HAVE A VOICE</a>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">REGION CHECK</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 b-wht">
    		<div class="spacer-30">&nbsp;</div>
    		
    		<div class="push-1 col-22">
                <% Html.RenderPartial("Message"); %>
			    <% Html.RenderPartial("Validation"); %>
				<div class="clear">&nbsp;</div>
				
			    <% using (Html.BeginForm("Index", "StreetCleaning", FormMethod.Post, new { @class = "create" })) { %>
	                <div class="col-4 m-rgt right">
	                	<label for="Title">North Street:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("NorthStreet") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">South Street:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("SouthStreet") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">West Street:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("WestStreet") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">East Street:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("EastStreet") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">Address To Check:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("Address") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">City:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("City", "Chicago") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>

	                <div class="col-4 m-rgt right">
	                	<label for="Title">State:</label>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="col-6">
	                	<%= Html.TextBox("State", "IL") %>
	                	<div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="spacer-10">&nbsp;</div>


					<div class="col-10 right">
						<input type="submit" value="Check" class="create" />
						<div class="clear">&nbsp;</div>
					</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
				<div class="spacer-30">&nbsp;</div>
    		</div>
    	</div>
    </div>
</asp:Content>

