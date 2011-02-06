<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
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

    			<% using (Html.BeginForm("ChangePassword", "Account", FormMethod.Post, new { @class = "create" })) { %>
    				<% Html.RenderPartial("Message"); %>
    				<%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>
    				<div class="clear">&nbsp;</div>
    				
    				<div class="push-3 col-16 fnt-14 teal m-btm10">
    					Use the form below to change your password.<br />
    					New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    				</div>
    				<div class="clear">&nbsp;</div>
    				
					<div class="col-4 m-rgt right">
						<label for="currentPassword">Current Password:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.Password("currentPassword") %>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
	                    <span class="req">
		                    <%= Html.ValidationMessage("currentPassword") %>
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
	                    <%= Html.Password("newPassword") %>
	                    <div class="clear">&nbsp;</div>
                    </div>
                    <div class="col-14 m-lft">
	                    <span class="req">
		                    <%= Html.ValidationMessage("newPassword") %>
	                    </span>
                    </div>

					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					<div class="clear">&nbsp;</div>

					<div class="col-4 m-rgt right">
						<label for="confirmPassword">Confirm New Password:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.Password("confirmPassword") %>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<span class="req">
		                    <%= Html.ValidationMessage("confirmPassword") %>
	                    </span>
	                    <div class="clear">&nbsp;</div>
					</div>
					
					<div class="clear">&nbsp;</div>
					
					<div class="col-8 right m-btm30 m-top10">
	                    <input type="submit" value="Change Password" />
	                    <div class="clear">&nbsp;</div>
                    </div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
		    </div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
