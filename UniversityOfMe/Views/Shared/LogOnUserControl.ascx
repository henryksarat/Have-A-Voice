<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" language="javascript">
    $(function () {
        $('#LoginEmail').focus();
    });
</script>

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
                <%: Html.TextBox("LoginEmail")%>
                <%: Html.ValidationMessage("LoginEmail", "*")%>
			</div> 
			<div class="input"> 
                <%: Html.Password("LoginPassword")%>
                <%: Html.ValidationMessage("LoginPassword", "*")%>
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
				<a href="/Password/Request">Forgot your password?</a> 
			</div> 
		</div> 
	</div> 
<% } %>