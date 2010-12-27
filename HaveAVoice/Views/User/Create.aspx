<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.CreateUserModelBuilder>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Registration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		$(function() {
			$("#meter div").fadeTo("fast", "0.2");
			$("input[type=password]").keyup(function() {
				var pStr = $(this).val();
				var str = 0;
				
				if (pStr.length > 4) { str++; }
				if (pStr.match("[a-z]")) { str++; }
				if (pStr.match("[A-Z]")) { str++; }
				if (pStr.match("[0-9]")) { str++; }
				if (pStr.match("[\!\@\#\$\%\^\&\*\(\)]")) { str++; }
				
				if (str == 0)
				{
					$("#meter div").fadeTo("fast", "0.2");
				}
				else if (str <= 2)
				{
					$("#meter div:not(.weak)").fadeTo("fast", "0.2");
					$("#meter div.weak").fadeTo("fast", "1");
				}
				else if (str <= 4)
				{
					$("#meter div:not(.okay)").fadeTo("fast", "0.2");
					$("#meter div.okay").fadeTo("fast", "1");
				}
				else if (str == 5)
				{
					$("#meter div:not(.good)").fadeTo("fast", "0.2");
					$("#meter div.good").fadeTo("fast", "1");					
				}
				
				return false;
			});
		});
	</script>
    
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CREATE</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="col-1">&nbsp;</div>
    		<div class="col-22">
    			<div class="spacer-30">&nbsp;</div>
    			
    			<% using (Html.BeginForm("","", FormMethod.Post, new { @class = "create" })) { %>
    				<%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    				<%= Html.Encode(ViewData["Message"]) %>
    			
	    			<div class="col-4 right">
	    				<label for="FullName">Full Name:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("FullName", Model.FullName()) %>
	    			</div>
	    			<div class="col-14">
	    				<%= Html.ValidationMessage("FullName", "*") %>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>
	    			
	    			<div class="col-4 right">
	    				<label for="Email">Email:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("Email", Model.Email()) %>
	    			</div>
	    			<div class="col-14">
	    				<%= Html.ValidationMessage("Email", "*") %>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>
	    			
	    			<div class="col-4 right">
	    				<label for="Username">Username:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("Username", Model.Username())%>
	    			</div>
	    			<div class="col-14">
	    				<%= Html.ValidationMessage("Username", "*") %>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>
	    			
	    			<div class="col-4 right">
	    				<label for="Password">Password:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.Password("Password") %>
	    				<div class="clear">&nbsp;</div>
		                <div id="meter">
		                    <div class="weak">WEAK</div>
		                    <div class="okay">OKAY</div>
		                    <div class="good">GOOD</div>
		                </div>
	    			</div>
	    			<div class="col-14">
	    				<%= Html.ValidationMessage("Password", "*") %>
	    			</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 right">
						<label for="DateOfBirth">Date of Birth:</label>
					</div>
					<div class="col-4">
						<%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
					</div>
					<div class="col-14">
						<%= Html.ValidationMessage("DateOfBirth", "*") %>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 right">
						<label for="City">City:</label>
					</div>
					<div class="col-4">
						<%= Html.TextBox("City", Model.City())%>
					</div>
					<div class="col-12">
						<%= Html.ValidationMessage("City", "*")%>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 right">
						<label for="State">State:</label>
					</div>
					<div class="col-4">
						<%= Html.DropDownList("State", Model.States())%>
					</div>
					<div class="col-14">
						<%= Html.ValidationMessage("State", "*")%>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 right">
						<label for="Captcha">Captcha:</label>
					</div>
					<div class="col-18">
						<%= CaptchaHelper.GenerateCaptcha()  %>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4">&nbsp;</div>
					<div class="col-8">
						<%= Html.TextArea("AgreementText", UserHelper.UserAgreement()) %>
					</div>
					<div class="col-10">&nbsp;</div>
					<div class="clear">&nbsp;</div>
					<div class="col-4 right">
						<%= Html.CheckBox("Agreement") %>
					</div>
					<div class="col-6">
						I agree with the <a href="#">Terms of Service</a>.
					</div>
					<div class="col-12">
						<%= Html.ValidationMessage("Agreement", "*", new { cols = "40", rows = "4", resize = "none" })%>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
                
                	<div class="col-8 right">
                		<input type="submit" class="create" value="Create" />
						<%= Html.ActionLink("Cancel", "Index", "", new { @class = "cancel" }) %>
                	</div>
                	<div class="clear">&nbsp;</div>
	                
    			<% } %>
    			
    			<div class="spacer-30">&nbsp;</div>
    		</div>
    		<div class="col-1">&nbsp;</div>
    		<div class="clear">&nbsp;</div>
    	</div>
    </div>
</asp:Content>