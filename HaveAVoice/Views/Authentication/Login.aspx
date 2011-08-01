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
			    	
					<% Html.RenderPartial("Message"); %>
			        <div class="push-5">
					    <div class="col-4 m-rgt right">
						    <label for="Email">Email:</label>
					    </div>
					    <div class="col-5">
						    <%= Html.TextBox("EmailLogin") %>
					    </div>
			            <div class="clear">&nbsp;</div>
			            <div class="spacer-10">&nbsp;</div>
			        
			            <div class="col-4 m-rgt right">
			                <label for="Password">Password:</label>
			           </div>
			           <div class="col-5">
			                <%= Html.Password("PasswordLogin") %>
			            </div>
					
					    <div class="clear">&nbsp;</div>
					    <div class="spacer-10">&nbsp;</div>
					    <div class="clear">&nbsp;</div>
  		                <div class="push-7 col-4">
					        <span class="fnt-10 v-alignmid">Remember Me </span><%= Html.CheckBox("RememberMe") %>
                        </div>
                        <div class="clear">&nbsp;</div>
					    <div class="push-3 col-8">
						    <div class="spacer-10">&nbsp;</div>
						    <div class="clear">&nbsp;</div>
						    <%=Html.ActionLink("Forgot Password", "Request", "Password", null, new{ @class="forgot" }) %> | <%=Html.ActionLink("Create Account", "Create", "User", null, new{ @class="forgot" }) %>
					    </div>
					    <div class="col-14">
						    &nbsp;
					    </div>

					    <div class="clear">&nbsp;</div>
					    <div class="spacer-10">&nbsp;</div>
					    <div class="clear">&nbsp;</div>
					
                	    <div class="push-7 col-4">
				            <input type="submit" class="create" value="Login" />
					    </div>
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
