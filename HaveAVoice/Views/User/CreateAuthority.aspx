<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.CreateAuthorityUserModelBuilder>" %>
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
	        $('#DateOfBirth').datepicker({
                yearRange: "-10:+10",
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm-dd-yy"
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
    			
    			<% using (Html.BeginForm("CreateAuthority", "User", FormMethod.Post, new { @class = "create" })) { %>
                    <% Html.RenderPartial("Message"); %>
    				<%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    				<div class="clear">&nbsp;</div>

                    <%= Html.Hidden("Email", Model.Email) %>
                    <%= Html.Hidden("Token", Model.Token) %>
                    <%= Html.Hidden("AuthorityType", Model.AuthorityType) %>
	    			
                    <div class="col-4 m-rgt right">
	    				<label for="FirstName">Email:</label>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-4">
	    				<%= Model.Email %>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>

	    			<div class="col-4 m-rgt right">
	    				<label for="FirstName">First Name:</label>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("FirstName", Model.FirstName) %>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-14 m-lft">
	    				<span class="req">
		    				<%= Html.ValidationMessage("FirstName", "*") %>
	    				</span>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>

	    			<div class="col-4 m-rgt right">
	    				<label for="LastName">Last Name:</label>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("LastName", Model.LastName) %>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-14 m-lft">
	    				<span class="req">
		    				<%= Html.ValidationMessage("LastName", "*") %>
	    				</span>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>
	    			    			
	    			<div class="col-4 m-rgt right">
	    				<label for="Gender">Gender:</label>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.DropDownList("Gender", Model.Genders) %>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="col-14 m-lft">
	    				<span class="req">
	    					<%= Html.ValidationMessage("Gender", "*") %>
	    				</span>
	    				<div class="clear">&nbsp;</div>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>

	    			<div class="col-4 m-rgt right">
	    				<label for="Password">Password:</label>
	    				<div class="clear">&nbsp;</div>
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
	    			<div class="col-14 m-lft">
	    				<span class="req">
		    				<%= Html.ValidationMessage("Password", "*") %>
	    				</span>
	    				<div class="clear">&nbsp;</div>
	    			</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="DateOfBirth">Date of Birth:</label>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<div class="req">
							<%= Html.ValidationMessage("DateOfBirth", "*") %>
						</div>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="City">Representing City:</label>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.TextBox("RepresentingCity", Model.RepresentingCity)%>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-12 m-lft">
						<span class="req">
							<%= Html.ValidationMessage("City", "*") %>
						</span>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="State">Representing State:</label>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.DropDownList("RepresentingState", Model.States) %>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<span class="req">
							<%= Html.ValidationMessage("State", "*") %>
						</span>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
										
					<div class="col-4">&nbsp;</div>
					<div class="col-8">
						<%= Html.TextArea("AgreementText", UserHelper.UserAgreement(), new { cols = "40", rows = "4", resize = "none" }) %>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-10">&nbsp;</div>
					<div class="clear">&nbsp;</div>
					<div class="col-4 m-rgt right">
						<%= Html.CheckBox("Agreement") %>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-6">
						I agree with the <a href="#">Terms of Service</a>.
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="col-12 m-lft">
						<span class="req">
							<%= Html.ValidationMessage("Agreement", "*") %>
						</span>
	    				<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
                
                	<div class="col-8 right">
                		<input type="submit" class="create" value="CreateUser" />
						<%= Html.ActionLink("Cancel", "Index", "", new { @class = "cancel" }) %>
	    				<div class="clear">&nbsp;</div>
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