<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NotLoggedInModel>" %>

<script type="text/javascript" language="javascript">
	$(function() {
		$("input.create").click(function(){
			window.location = "/User/Create";
		});
	});
</script>

<div class="push-1 col-22 sign-up">
	<div class="col-16 center padding-22t">
		<h3>
			Connecting you to the political world instantaneously.
		</h3>
	</div>
	
	<div class="col-6 center">
		<input type="button" class="create" value="Create a new account" />
	</div>
	<div class="clear">&nbsp;</div>
</div>
<div class="clear">&nbsp;</div>