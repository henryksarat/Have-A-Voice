<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
	<script type="text/javascript" language="javascript">
		$(function() {				
			function equalHeight(group) {
				var tallest = 0;
				group.each(function() {
					var thisHeight = $(this).height();
					if(thisHeight > tallest) {
						tallest = thisHeight;
					}
				});
				group.height(tallest);
			}
			$("form label").click(function() {
				var inpt = $(this).prev("input");
				if (inpt.val() == '') {
					$(this).fadeTo('fast', 0.4);
				} else {
					$(this).fadeTo('fast', 0.0);
				}
				$(inpt).focus();
			});
			$("form input").bind("focus", function() {
				var lbl = $(this).next("label");
				if ($(this).val() == '') {
					lbl.fadeTo('fast', 0.4);
				} else {
					lbl.fadeTo('fast', 0.0);
				}
			}).bind("blur", function() {
				var lbl = $(this).next("label");
				if ($(this).val() == '') {
					lbl.fadeTo('fast', 1.0);
				} else {
					lbl.fadeTo('fast', 0.0)
				}
			}).bind("keypress", function() {
				var lbl = $(this).next("label");
				if ($(this).val() == '') {
					lbl.fadeTo('fast', 0.4);
				} else {
					lbl.fadeTo('fast', 0.0);
				}
			});
			
			equalHeight($("div[rel=match]"));
		});
	</script>

    <div class="col-18">
    <% using (Html.BeginForm("Login", "Authentication")) {%>
        <div class="col-2 sign-in">
            Sign In
        </div>
        <div class="col-15">
            <div class="col-5 rel">
                <%= Html.TextBox("Email", null, new{@class = "login"})%>
                <%= Html.ValidationMessage("Email", "*")%>
		        <label>username</label>
                <div class="remember rel">
		            remember me
		            <%= Html.CheckBox("RememberMe") %>
                </div>
            </div>
            <div class="col-5 rel">
                <%= Html.Password("Password", null, new { @class = "login" })%>
                <%= Html.ValidationMessage("Password", "*")%>
                <label>password</label>
                <br />
                <%=Html.ActionLink("forgot password?", "ForgotPassword", null, new{@class="forgot"}) %>
            </div>

            <div class="col-5">
                <input type="submit" value="Login" class="button" />
            </div>
        </div>
    <% } %>

    <!-- Logon: -->
    <!--[ <%/*= Html.ActionLink("Log In", "Login", "Authentication")*/%> ] -->
    </div>
<% } else { %>
    <div class="col-12">
        <ul>
            <li><a href="#">HOME</a></li>
			<li><a href="#">FRIENDS</a></li>
			<li><a href="#">MAIL</a></li>
			<li><a href="#">NOTIFICATIONS</a></li>
        </ul>
    </div>
    <div class="col-6">
	    <ul class="right">
            <li><a href="#">SETTINGS</a></li>
            <li><%= Html.ActionLink("LOGOUT", "LogOut", "Authentication")%></li>
        </ul>
	</div>
<% } %>