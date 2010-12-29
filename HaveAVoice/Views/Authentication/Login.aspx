<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">LOGIN</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="col-1">&nbsp;</div>
    		<div class="col-22">
    			<div class="spacer-30">&nbsp;</div>

			    <% using (Html.BeginForm("Login", "Authentication", FormMethod.Post, new { @class = "create" })) { %>
			        <%= Html.Encode(ViewData["Message"]) %>
			
					<div class="col-4 m-rgt right">
						<label for="Email">Username:</label>
					</div>
					<div class="col-4">
						<%= Html.TextBox("Username") %>
					</div>
					<div class="col-14">
						<%= Html.ValidationMessage("Username", "*") %>
					</div>
			        <div class="clear">&nbsp;</div>
			        <div class="spacer-10">&nbsp;</div>
			        
			        <div class="col-4 m-rgt right">
			            <label for="Password">Password:</label>
			       </div>
			       <div class="col-4">
			            <%= Html.Password("Password") %>
			        </div>
			        <div class="col-14">
			            <%= Html.ValidationMessage("Password", "*") %>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="RememberMe">Remember me:</label>
					</div>
					<div class="col-4">
						<%= Html.CheckBox("RememberMe") %>
						<div class="clear">&nbsp;</div>
						<div class="spacer-10">&nbsp;</div>
						<a href="#">Forgot Password</a>
					</div>
					<div class="col-14">
						&nbsp;
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
                	<div class="col-8 right">
				        <input type="submit" class="create" value="Login" />
				        <%= Html.ActionLink("Cancel", "Index", "", new { @class = "cancel" }) %>
					</div>
			    <% } %>
			    
			    <div class="clear">&nbsp;</div>
			    <div class="spacer-30">&nbsp;</div>
		   </div>
		   <div class="col-1">&nbsp;</div>
		   <div class="clear">&nbsp;</div>
	   </div>
    </div>
</asp:Content>
