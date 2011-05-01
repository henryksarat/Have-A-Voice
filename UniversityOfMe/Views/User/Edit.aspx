<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EditUserModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - Edit Your profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#DateOfBirth').datepicker({
                yearRange: '1900:2011',
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm-dd-yy"
            });
        });    
    </script>
    
	<% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>
    
	<img src="<%= Model.Get().ProfilePictureURL %>" /><br />

	Name: <%= Model.Get().OriginalFullName %><br />
	Gender: <%= Model.Get().OriginalGender%><br />
	Site: <%= Model.Get().OriginalWebsite%><br />
	Email: <%= Model.Get().OriginalEmail%><br />

    About: <br />
	<%= Model.Get().AboutMe%><br /><br />
	
	<% using(Html.BeginForm("Edit", "User")) { %>
        <%= Html.Hidden("Id", Model.Get().Id)%>
	    <%= Html.Hidden("OriginalEmail", Model.Get().OriginalEmail)%>
        <%= Html.Hidden("OriginalFullName", Model.Get().OriginalFullName)%>
        <%= Html.Hidden("OriginalGender", Model.Get().OriginalGender)%>
        <%= Html.Hidden("OriginalWebSite", Model.Get().OriginalWebsite)%>
	    <%= Html.Hidden("OriginalPassword", Model.Get().OriginalPassword)%>
	    <%= Html.Hidden("UserId", Model.Get().Id)%>
	    <%= Html.Hidden("ProfilePictureURL", Model.Get().ProfilePictureURL)%>
	
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
			<%= Html.TextBox("FirstName", Model.Get().FirstName) %>
			<span class="req">
		        <%= Html.ValidationMessage("FirstName", "*") %>
	        </span>
		</div>
	    <div class="col-18 spacer-15">&nbsp;</div>
	    <div class="col-6">
	        <label>Last Name:</label>
	    </div>
	    <div class="push-1 col-6">
	        <%= Html.TextBox("LastName", Model.Get().LastName)%>
	        <span class="req">
		        <%= Html.ValidationMessage("LastName", "*") %>
	        </span>
	    </div>
		<div class="col-18 spacer-15">&nbsp;</div>
	    <div class="col-6">
	        <label>Date of Birth:</label>
	    </div>
	    <div class="push-1 col-6">
	        <%= Html.TextBox("DateOfBirth", String.Format("{0:g}", Model.Get().DateOfBirth)) %>
	        <span class="req">
		        <%= Html.ValidationMessage("DateOfBirth", "*") %>
	        </span>
	    </div>
	    <div class="col-18 spacer-15">&nbsp;</div>
		<div class="col-6">
			<label>City:</label>
		</div>
		<div class="push-1 col-6">
	        <%= Html.TextBox("City", Model.Get().City) %>
	        <span class="req">
		        <%= Html.ValidationMessage("City", "*") %>
	        </span>
		</div>
	    <div class="col-18 spacer-15">&nbsp;</div>
	    <div class="col-6">
	        <label>State:</label>
	    </div>
	    <div class="push-1 col-6">
	        <%= Html.DropDownList("State", Model.Get().States) %>
	        <span class="req">
		        <%= Html.ValidationMessage("State", "*") %>
	        </span>
	    </div>
	    <div class="col-18 spacer-15">&nbsp;</div>
		<div class="col-6">
			<label>Website:</label>
		</div>
		<div class="push-1 col-6">
	        <%= Html.TextBox("Website", Model.Get().Website)%>
	        <span class="req">
		        <%= Html.ValidationMessage("Website", "*") %>
			</span>
		</div>
	    <div class="col-18 spacer-15">&nbsp;</div>
	    <div class="col-6">
	        <label>New Email:</label>
	    </div>
	    <div class="push-1 col-6">
	        <%= Html.TextBox("NewEmail", Model.Get().NewEmail)%>
	        <span class="req">
	            <%= Html.ValidationMessage("NewEmail", "*") %>
	        </span>
	    </div>
	    <div class="col-18 spacer-15">&nbsp;</div>
	    <div class="col-6">
	        <label>Gender:</label>
	    </div>
	    <div class="push-1 col-6">
	        <%= Html.DropDownList("Gender", Model.Get().Genders) %>
	        <span class="req">
		        <%= Html.ValidationMessage("Gender", "*") %>
	        </span>
	    </div>
	    <div class="col-18 spacer-15">&nbsp;</div>
		<div class="col-6">
			<label>About Me:</label>
		</div>
		<div class="push-1 col-6">
	        <%= Html.TextArea("AboutMe", Model.Get().AboutMe, new { style = "width:300px; height: 200px" }) %>
	        <span class="req">
		        <%= Html.ValidationMessage("AboutMe", "*") %>
	        </span>
		</div>
	    <div class="col-18 spacer-15">&nbsp;</div>
		<div class="push-7 col-4">
			<input type="submit" class="button" value="Save" />
	    </div>
    <% } %>
</asp:Content>