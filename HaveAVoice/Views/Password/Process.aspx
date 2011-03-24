<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.Generic.Models.StringModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Change Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-6 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CHANGE PASSWORD</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="col-1">&nbsp;</div>
    		<div class="col-22">
    			<div class="spacer-30">&nbsp;</div>

    			<% using (Html.BeginForm("Process", "Password", FormMethod.Post, new { @class = "create" })) { %>
    				<%= Html.Hidden("ForgotPasswordHash", Model.Value) %>
    				<% Html.RenderPartial("Message"); %>
    				<% Html.RenderPartial("Validation"); %>
    				<div class="clear">&nbsp;</div>
    				
    				<div class="push-3 col-16 fnt-14 teal m-btm10">
    					Use the form below to change your password.<br />
    					New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    				</div>
    				<div class="clear">&nbsp;</div>
    				
					<div class="col-4 m-rgt right">
						<label for="currentPassword">Email:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.TextBox("Email") %>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
	                    <span class="req">
		                    <%= Html.ValidationMessage("Email", "*") %>
	                    </span>
	                    <div class="clear">&nbps;</div>
					</div>

					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					<div class="clear">&nbsp;</div>

					<div class="col-4 m-rgt right">
	                    <label for="newPassword">New Password:</label>
                    </div>
                    <div class="col-4">
	                    <%= Html.Password("Password") %>
	                    <div class="clear">&nbsp;</div>
                    </div>
                    <div class="col-14 m-lft">
	                    <span class="req">
		                    <%= Html.ValidationMessage("Password") %>
	                    </span>
                    </div>

					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					<div class="clear">&nbsp;</div>

					<div class="col-4 m-rgt right">
						<label for="confirmPassword">Retype Password:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.Password("RetypedPassword") %>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<span class="req">
		                    <%= Html.ValidationMessage("RetypedPassword", "*") %>
	                    </span>
	                    <div class="clear">&nbsp;</div>
					</div>
					
					<div class="clear">&nbsp;</div>
					
					<div class="col-8 right m-btm30 m-top10">
	                    <input type="submit" value="Submit" />
	                    <div class="clear">&nbsp;</div>
                    </div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
		    </div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>