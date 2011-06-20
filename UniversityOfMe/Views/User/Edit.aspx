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
    
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner title black full red-top small"> 
			<span class="edit">EDIT YOUR ACCOUNT</span> 
        </div>
        <div class="flft pl50 pt30 wp20">
            <div class="center">
                <img src="<%= Model.Get().ProfilePictureURL %>" />
            </div>
            <div>
                <span class="bold">Name:</span> <%= Model.Get().OriginalFullName %>
            </div>
            <div>
                <span class="bold"">Gender:</span> <%= Model.Get().OriginalGender%>
            </div>
            <div>
                <span class="bold">Email:</span> <%= Model.Get().OriginalEmail%>
            </div>
            <div>
                <span class="bold">About:</span> <br /><%= String.IsNullOrEmpty(Model.Get().AboutMe) ? "NA" : Model.Get().AboutMe %>
            </div>
        </div>
        <div  class="create frgt pt30 pr150">
            <div>
                <% using (Html.BeginForm("Edit", "User")) { %>
                    <%= Html.Hidden("Id", Model.Get().Id)%>
	                <%= Html.Hidden("OriginalEmail", Model.Get().OriginalEmail)%>
                    <%= Html.Hidden("OriginalFullName", Model.Get().OriginalFullName)%>
                    <%= Html.Hidden("OriginalGender", Model.Get().OriginalGender)%>
                    <%= Html.Hidden("OriginalWebSite", Model.Get().OriginalWebsite)%>
	                <%= Html.Hidden("OriginalPassword", Model.Get().OriginalPassword)%>
	                <%= Html.Hidden("UserId", Model.Get().Id)%>
	                <%= Html.Hidden("ProfilePictureURL", Model.Get().ProfilePictureURL)%>

			        <label for="NewPassword">New Password:</label>
			        <%= Html.Password("NewPassword")%>
		            <%= Html.ValidationMessage("NewPassword", "*")%>
            </div>
            <div>
			        <label for="RetypedPassword">Confirm New Password:</label>
			        <%= Html.Password("RetypedPassword")%>
		            <%= Html.ValidationMessage("RetypedPassword", "*")%>
            </div>
            <div>
                    <label for="FirstName">First Name:</label>
			        <%= Html.TextBox("FirstName", Model.Get().FirstName) %>
		            <%= Html.ValidationMessage("FirstName", "*") %>
            </div>
            <div>
                    <label for="LastName">Last Name:</label>
                    <%= Html.TextBox("LastName", Model.Get().LastName)%>
		            <%= Html.ValidationMessage("LastName", "*") %>
	        </div>
            <div>
                    <label for="DateOfBirth">Date of Birth:</label>
	                <%= Html.TextBox("DateOfBirth", String.Format("{0:g}", Model.Get().DateOfBirth)) %>
		            <%= Html.ValidationMessage("DateOfBirth", "*") %>
	        </div>
            <div>
                    <label for="City">City:</label>
	                <%= Html.TextBox("City", Model.Get().City) %>
		            <%= Html.ValidationMessage("City", "*") %>
	        </div>
            <div>
                    <label for="State">State:</label>
	                <%= Html.DropDownList("State", Model.Get().States) %>
		            <%= Html.ValidationMessage("State", "*") %>
	        </div>
            <div>
                    <label for="State">Relationship Status:</label>
	                <%= Html.DropDownList("RelationshipStatus", Model.Get().RelationshipStatu) %>
	        </div>
            <div>
                    <label for="Website">Website:</label>
	                <%= Html.TextBox("Website", Model.Get().Website)%>
		            <%= Html.ValidationMessage("Website", "*") %>
	        </div>
            <div>
                    <label for="NewEmail">New Email:</label>
	                <%= Html.TextBox("NewEmail", Model.Get().NewEmail)%>
	                <%= Html.ValidationMessage("NewEmail", "*") %>
	        </div>
            <div>
                    <label for="Gender">Gender:</label>
	                <%= Html.DropDownList("Gender", Model.Get().Genders) %>
		            <%= Html.ValidationMessage("Gender", "*") %>
	        </div>
            <div>
                    <label for="AboutMe">About Me:</label>
	                <%= Html.TextArea("AboutMe", Model.Get().AboutMe, 15, 35, null) %>
		            <%= Html.ValidationMessage("AboutMe", "*") %>
	        </div>
            <div class="right"> 
                <input type="submit" value="Save" class="btn site" />
            </div>

                <% } %>
        </div>
    </div>
</asp:Content>