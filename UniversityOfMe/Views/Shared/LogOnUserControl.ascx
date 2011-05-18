<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% using (Html.BeginForm("Login", "Authentication", FormMethod.Post)) { %>
	<div class="login"> 
		<div class="s-row"> 
			<div class="input"> 
				<label for="username">Email</label> 
			</div> 
			<div class="input"> 
				<label for="password">Password</label> 
			</div> 
		</div> 
		<div class="s-row"> 
			<div class="input"> 
                <%: Html.TextBox("Email")%>
                <%: Html.ValidationMessage("Email", "*")%>
			</div> 
			<div class="input"> 
                <%: Html.Password("Password") %>
                <%: Html.ValidationMessage("Password", "*")%>
			</div> 
			<div class="input half"> 
				<input type="submit" name="submit" class="btn" value="Login" /> 
			</div> 
		</div> 
		<div class="s-row"> 
			<div class="input"> 
                <%= Html.CheckBox("RememberMe") %> Keep me logged in
			</div> 
			<div class="input right"> 
				<a href="#">Forgot your password?</a> 
			</div> 
		</div> 
	</div> 
<% } %>