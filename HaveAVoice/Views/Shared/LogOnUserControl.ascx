<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript" language="javascript">
    $(function () {
        $("#EmailLogin").click(function () {
            $("#EmailLogin").val("");
        });
    });
</script>

<div class="rcol">
	<div class="frm">
        <% using (Html.BeginForm("Login", "Authentication")) { %>
        <%= Html.TextBox("EmailLogin", "Email", new { @class = "txt" })%>
        <%= Html.Password("PasswordLogin", string.Empty, new { @class = "txt" }) %>
		<input type="submit" class="btn" value="Login" />
		<br />
		<div class="optn">
			<%= Html.CheckBox("RememberMe") %>
			Keep me logged in
		</div>
		<div class="optn">
            <%=Html.ActionLink("forgot password?", "Request", "Password") %>
		</div>
        <% } %>
	</div>
</div>