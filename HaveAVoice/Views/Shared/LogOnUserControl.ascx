<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

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
		
		$("form input.login").each(function() {
			if ($(this).val() == '')
			{
				$(this).next("label").fadeTo('fast', 1.0);
			} else {
				$(this).next("label").fadeTo('fast', 0.0);
			}
		});

		$("form label").click(function() {
			var inpt = $(this).prev("input");
			if (inpt.val() == '') {
				$(this).fadeTo('fast', 0.4);
			} else {
				$(this).fadeTo('fast', 0.0);
			}
			$(inpt).focus();
		});

		$("form input.login").bind("focus", function() {
			var lbl = $(this).next("label");
			if ($(this).val() == '')
			{
				lbl.fadeTo('fast', 0.4);
			} else {
				lbl.fadeTo('fast', 0.0);
			}
		}).bind("blur", function() {
			var lbl = $(this).next("label");
			if ($(this).val() == '')
			{
				lbl.fadeTo('fast', 1.0);
			} else {
				lbl.fadeTo('fast', 0.0);
			}
		}).bind("keypress", function() {
			var lbl = $(this).next("label");
			if ($(this).val() == '')
			{
				lbl.fadeTo('fast', 0.4);
			} else {
				lbl.fadeTo('fast', 0.0);
			}
		});
		
		$("img.profile").load(function() {
			equalHeight($("div[rel=match]"));
		});
		
	});
</script>

<div class="col-18">
	<% using (Html.BeginForm("Login", "Authentication")) { %>
	    <div class="col-2 sign-in">
	        Sign In
	        <div class="clear">&nbsp;</div>
	    </div>
	    <div class="col-16">
	        <div class="col-5 rel">
	            <%= Html.TextBox("Email", null, new{ @class = "login" }) %>
			    <label>email</label>
	            <div class="remember rel">
			        remember me
			        <%= Html.CheckBox("RememberMe") %>
	            </div>
	            <div class="clear">&nbsp;</div>
	        </div>
	        <div class="col-5 rel">
	            <%= Html.Password("Password", null, new { @class = "login" }) %>
	            <label>password</label>
	            <div class="clear">&nbsp;</div>
	            <div class="spacer-2">&nbsp;</div>
	            <%=Html.ActionLink("forgot password?", "Request", "Password", null, new{ @class="forgot" }) %>
	            <div class="clear">&nbsp;</div>
	        </div>
	
	        <div class="col-3">
	            <input type="submit" value="Login" class="button" />
	            <div class="clear">&nbsp;</div>
	        </div>
	    </div>
	<% } %>
	<div class="clear">&nbsp;</div>
</div>