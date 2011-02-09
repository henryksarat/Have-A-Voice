<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.EditUserModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function() {
	        $('#DateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm-dd-yy"
            });
        });    
    </script>
    
    <div class="container-24">
	    <div class="col-6 profile m-top30">
	        <img src="<%= Model.ProfilePictureURL %>" alt="<%= NameHelper.FullName(Model.UserInformation) %>" class="profile" />
	        <br />
	        <div class="col-6 p-v10 details">
				<span class="blue">Name:</span> <%= Model.OriginalFullName %><br />
				<span class="blue">Gender:</span> <%= Model.OriginalGender %><br />
				<span class="blue">Site:</span> <%= Model.OriginalWebsite %><br />
	            <span class="blue">Email:</span> <%= Model.OriginalEmail %><br />
			</div>

	        <div class="col-6">
			    <h4>About Me</h4>
	            <%= Model.UserInformation.AboutMe %>
	        </div>
	    </div>
	
		<div class="m-lft col-18 form">
	        <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
	
	        <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
			<% Html.RenderPartial("PrivacyTabs", UserSettings.AccountSettings); %>
					
			<div class="clear">&nbsp;</div>

			<% using(Html.BeginForm("Edit", "User", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
	            <%= Html.Hidden("OriginalEmail", Model.OriginalEmail) %>
                <%= Html.Hidden("OriginalFullName", Model.OriginalFullName) %>
                <%= Html.Hidden("OriginalGender", Model.OriginalGender) %>
                <%= Html.Hidden("OriginalWebSite", Model.OriginalWebsite) %>
	            <%= Html.Hidden("OriginalPassword", Model.OriginalPassword) %>
	            <%= Html.Hidden("UserId", Model.UserInformation.Id) %>
	            <%= Html.Hidden("ProfilePictureURL", Model.ProfilePictureURL) %>
	
	            <div class="col-6">
					<label>Enter a new password:</label>
	                If you wish to change your password,<br />
					enter the new password here.
				</div>
				<div class="push-1 col-6">
					<%= Html.Password("NewPassword") %>
					<span class="req">
		                <%= Html.ValidationMessage("NewPassword", "*") %>
	                </span>
				</div>
				<div class="col-18 spacer-15">&nbsp;</div>
				<div class="col-6">
					<label>Confirm new password:</label>
	            </div>
				<div class="push-1 col-6">
					<%= Html.Password("RetypedPassword") %>
					<span class="req">
		                <%= Html.ValidationMessage("RetypedPassword", "*") %>
	                </span>
				</div>
				<div class="col-18 spacer-15">&nbsp;</div>
				<div class="col-6">
					<label>First Name:</label>
				</div>
	            <div class="push-1 col-6">
					<%= Html.TextBox("FirstName", Model.UserInformation.FirstName) %>
					<span class="req">
		                <%= Html.ValidationMessage("FirstName", "*") %>
	                </span>
				</div>
	            <div class="col-18 spacer-15">&nbsp;</div>
	            <div class="col-6">
	                <label>Last Name:</label>
	            </div>
	            <div class="push-1 col-6">
	                <%= Html.TextBox("LastName", Model.UserInformation.LastName) %>
	                <span class="req">
		                <%= Html.ValidationMessage("LastName", "*") %>
	                </span>
	            </div>
				<div class="col-18 spacer-15">&nbsp;</div>
	            <div class="col-6">
	                <label>Date of Birth:</label>
	            </div>
	            <div class="push-1 col-6">
	                <%= Html.TextBox("DateOfBirth", String.Format("{0:g}", Model.UserInformation.DateOfBirth)) %>
	                <span class="req">
		                <%= Html.ValidationMessage("DateOfBirth", "*") %>
	                </span>
	            </div>
	            <div class="col-18 spacer-15">&nbsp;</div>
				<div class="col-6">
					<label>City:</label>
				</div>
				<div class="push-1 col-6">
	                <%= Html.TextBox("City", Model.UserInformation.City) %>
	                <span class="req">
		                <%= Html.ValidationMessage("City", "*") %>
	                </span>
				</div>
	            <div class="col-18 spacer-15">&nbsp;</div>
	            <div class="col-6">
	                <label>State:</label>
	            </div>
	            <div class="push-1 col-6">
	                <%= Html.DropDownList("State", Model.States) %>
	                <span class="req">
		                <%= Html.ValidationMessage("State", "*") %>
	                </span>
	            </div>
	            <div class="col-18 spacer-15">&nbsp;</div>
				<div class="col-6">
					<label>Website:</label>
				</div>
				<div class="push-1 col-6">
	                <%= Html.TextBox("Website", Model.UserInformation.Website) %>
	                <span class="req">
		                <%= Html.ValidationMessage("Website", "*") %>
					</span>
				</div>
	            <div class="col-18 spacer-15">&nbsp;</div>
	            <div class="col-6">
	                <label>Email:</label>
	            </div>
	            <div class="push-1 col-6">
	                <%= Html.TextBox("Email", Model.UserInformation.Email) %>
	                <span class="req">
	                	<%= Html.ValidationMessage("Email", "*") %>
	                </span>
	            </div>
	            <div class="col-18 spacer-15">&nbsp;</div>
	            <div class="col-6">
	                <label>Gender:</label>
	            </div>
	            <div class="push-1 col-6">
	                <%= Html.DropDownList("Gender", Model.Genders) %>
	                <span class="req">
		                <%= Html.ValidationMessage("Gender", "*") %>
	                </span>
	            </div>
	            <div class="col-18 spacer-15">&nbsp;</div>
				<div class="col-6">
					<label>About Me:</label>
				</div>
				<div class="push-1 col-6">
	                <%= Html.TextArea("AboutMe", Model.UserInformation.AboutMe, new { style = "width:300px; height: 200px" }) %>
	                <span class="req">
		                <%= Html.ValidationMessage("AboutMe", "*") %>
	                </span>
				</div>
	            <div class="col-18 spacer-15">&nbsp;</div>
				<div class="push-7 col-4">
					<input type="submit" class="button" value="Save" />
	                <input type="button" class="button" value="Cancel" />
	            </div>
            <% } %>
		</div>
        <div class="clear">&nbsp;</div>
	</div>
	<div class="clear">&nbsp;</div>
</asp:Content>