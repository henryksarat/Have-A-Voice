<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EditUserModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Your Profile
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

        <div class="edit-user-form">
		<div class="banner title black full small"> 
			<span class="edit">EDIT YOUR ACCOUNT</span> 
        </div>

        <div style="padding-top: 25px">
            <% Html.RenderPartial("Validation"); %>
        </div>

        <div class="twoColEditUser clearfix">
            <div class="lCol">     
                <div>
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
            </div>
            <div class="rCol">     
                <div  class="create pt30 pr150">
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
            </div>
                <% if (string.IsNullOrEmpty(Model.Get().ShortUrl)) { %>
                    <div class="field-holder" style="margin-bottom:40px">
                        <div class="center small bold">
                            Let people access your profile easier with a UniversityOf.Me<sup>&trade;</sup> URL. <br />
                            NOTE: Once you set this it can no longer be changed.
                        </div>
                        <div class="center bold">
			                www.universityof.me/
			                <%= Html.TextBox("ShortUrl")%>
		                    <%= Html.ValidationMessage("ShortUrl", "*", new { @class = "req" })%>
                        </div>
                </div>
                <% } else { %>
                    <div class="field-holder" style="margin-bottom:40px">
                        <div class="center small bold">
                            People can easily access your profile by going here:
                        </div>
                        <div class="center bold">
			                www.universityof.me/<%= Model.Get().ShortUrl %>
                        </div>
                </div>
                <% } %>
                <div class="field-holder">
			        <label for="NewPassword">New Password</label>
			        <%= Html.Password("NewPassword")%>
		            <%= Html.ValidationMessage("NewPassword", "*", new { @class = "req" })%>
                </div>
                <div class="field-holder">
			        <label for="RetypedPassword">Confirm New Password</label>
			        <%= Html.Password("RetypedPassword")%>
		            <%= Html.ValidationMessage("RetypedPassword", "*")%>
                </div>
                <div class="field-holder">
                    <label for="FirstName">First Name</label>
			        <%= Html.TextBox("FirstName", Model.Get().FirstName) %>
		            <%= Html.ValidationMessage("FirstName", "*", new { @class = "req" })%>
                </div>
                <div class="field-holder">
                    <label for="LastName">Last Name</label>
                    <%= Html.TextBox("LastName", Model.Get().LastName)%>
		            <%= Html.ValidationMessage("LastName", "*", new { @class = "req" })%>
	            </div>
                <div class="field-holder">
                    <label for="DateOfBirth">Date of Birth</label>
	                <%= Html.TextBox("DateOfBirth", Model.Get().getDateOfBirthFormatted()) %>
		            <%= Html.ValidationMessage("DateOfBirth", "*", new { @class = "req" })%>
	            </div>
                <div class="field-holder">
                    <label for="City">City</label>
	                <%= Html.TextBox("City", Model.Get().City) %>
	            </div>
                <div class="field-holder">
                    <label for="State">State</label>
	                <%= Html.DropDownList("State", Model.Get().States) %>
	            </div>
                <div class="field-holder">
                    <label for="State">Relationship Status</label>
	                <%= Html.DropDownList("RelationshipStatus", Model.Get().RelationshipStatu)%>
	            </div>
                <div class="field-holder">
                    <label for="Job">Job</label>
	                <%= Html.TextBox("Job", Model.Get().Job)%>
	            </div>
                <div class="field-holder">
                    <label for="Website">Website</label>
	                <%= Html.TextBox("Website", Model.Get().Website)%>
	            </div>
                <div class="field-holder">
                    <label for="NewEmail">New Email</label>
	                <%= Html.TextBox("NewEmail", Model.Get().NewEmail)%>
	                <%= Html.ValidationMessage("NewEmail", "*", new { @class = "req" })%>
	            </div>
                <div class="field-holder">
                    <label for="Gender">Gender</label>
	                <%= Html.DropDownList("Gender", Model.Get().Genders) %>
		            <%= Html.ValidationMessage("Gender", "*", new { @class = "req" })%>
	            </div>
                <div class="field-holder">
                    <label for="AboutMe">About Me</label>
	                <%= Html.TextArea("AboutMe", Model.Get().AboutMe, 15, 35, new { @class ="textarea" })%>
	            </div>
                <div class="field-holder center ml190">
                    <input type="submit" value="Save" class="btn site" />
                </div>

                <% } %>
                </div>
            </div>
        </div>
        </div>
    </div>
</asp:Content>