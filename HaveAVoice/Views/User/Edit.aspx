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
                yearRange: "-10:+10",
                changeMonth: true,
                changeYear: true
            });
        });    
    </script>
    
    <div class="grid_7 omega profile" rel="match">
        <img src="<%= Model.ProfilePictureURL %>" alt="<%= Model.UserInformation.Username %>" height="154" width="154" class="profile" />
        <br />
        <div class="grid_6 padding-10v details">
			<span class="blue">Name:</span> <%=Model.UserInformation.FirstName + " " + Model.UserInformation.LastName %><br />
			<span class="blue">Gender:</span> GENDER<br />
			<span class="blue">Site:</span> <%=Model.UserInformation.Website %><br />
            <span class="blue">Email:</span> <%=Model.UserInformation.Email %><br />
		</div>

		<div class="alpha grid_6 round-3 margin-12b padding-10v padding-10h">
			<input type="button" class="friend" value="Become a friend" />
			<a class="padding-5v margin-15t" href="#">Send <%=Model.UserInformation.FirstName %> a private message</a>
			<h6 class="margin-15t">Stats</h6>
			<hr />
			<div class="alpha grid_1 teal font-18">32</div><div class="grid_1 white font-18">Ideas</div>
			<div class="clear">&nbsp;</div>
			<div class="alpha grid_6 font-12 padding-12v">
			    <span class="green">102 likes</span>
			    <span class="teal margin-12h">|</span>
			    <span class="red">1024 dislikes</span>
            </div>
		    <div class="clear">&nbsp;</div>
		    <div class="alpha grid_1 teal font-18"><%=Model.UserInformation.FriendedBy.Count %></div><div class="grid_1 white font-18">Friends</div>
	    </div>
        <div class="alpha grid_6">
		    <h4>About Me</h4>
            <%=Model.UserInformation.AboutMe %>
        </div>
    </div>

    <div class="alpha grid_13 omega form" rel="match">
        <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
        <% Html.RenderPartial("PrivacyTabs", UserSettings.AccountSettings); %>
				
		<div class="grid_13">&nbsp;</div>
            <%= Html.Encode(ViewData["Message"]) %>
            <% using(Html.BeginForm("Edit", "User", FormMethod.Post, new { enctype = "multipart/form-data" })) {%>

            <%= Html.Hidden("OriginalEmail", Model.OriginalEmail) %>
            <%= Html.Hidden("OriginalUsername", Model.OriginalUsername) %>
            <%= Html.Hidden("OriginalPassword", Model.OriginalPassword) %>
            <%= Html.Hidden("UserId", Model.UserInformation.Id) %>
            <%= Html.Hidden("ProfilePictureURL", Model.ProfilePictureURL) %>

            <div class="grid_6">
				<label>Enter a new password:</label>
                If you wish to change your password,<br />
				enter the new password here.
			</div>
			<div class="push_1 grid_6">
				<%= Html.Password("NewPassword") %>
                <%= Html.ValidationMessage("NewPassword", "*")%>
			</div>
			<div class="grid_13 spacer-16">&nbsp;</div>
			<div class="grid_6">
				<label>Confirm new password:</label>
            </div>
			<div class="push_1 grid_6">
				<%= Html.Password("RetypedPassword") %>
                <%= Html.ValidationMessage("RetypedPassword", "*")%>
			</div>
			<div class="grid_13 spacer-16">&nbsp;</div>
			<div class="grid_6">
				<label>First Name:</label>
			</div>
            <div class="push_1 grid_6">
				<%= Html.TextBox("FirstName", Model.UserInformation.FirstName) %>
                <%= Html.ValidationMessage("FirstName", "*") %>
			</div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>Last Name:</label>
            </div>
            <div class="push_1 grid_6">
                <%= Html.TextBox("LastName", Model.UserInformation.LastName)%>
                <%= Html.ValidationMessage("LastName", "*") %>
            </div>
			<div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>Date of Birth:</label>
            </div>
            <div class="push_1 grid_6">
                <%= Html.TextBox("DateOfBirth", String.Format("{0:g}", Model.UserInformation.DateOfBirth))%>
                <%= Html.ValidationMessage("DateOfBirth", "*") %>
            </div>
            <div class="grid_13 spacer-16">&nbsp;</div>
			<div class="grid_6">
				<label>City:</label>
			</div>
			<div class="push_1 grid_6">
                <%= Html.TextBox("City", Model.UserInformation.City)%>
                <%= Html.ValidationMessage("City", "*")%>
			</div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>State:</label>
            </div>
            <div class="push_1 grid_6">
                <%= Html.DropDownList("State", Model.States)%>
                <%= Html.ValidationMessage("State", "*")%>
            </div>
            <div class="grid_13 spacer-16">&nbsp;</div>
			<div class="grid_6">
				<label>Website:</label>
			</div>
			<div class="push_1 grid_6">
                <%= Html.TextBox("Website", Model.UserInformation.Website)%>
                <%= Html.ValidationMessage("Website", "*")%>
			</div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>Timezone:</label>
            </div>
            <div class="push_1 grid_6">
                <%= Html.DropDownList("Timezone", Model.Timezones)%>
            </div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>Email:</label>
            </div>
            <div class="push_1 grid_6">
                <%= Html.TextBox("Email", Model.UserInformation.Email)%>
                <%= Html.ValidationMessage("Email", "*") %>
            </div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
				<label>Avatar:</label>
				If you wish to set or change your avatar<br />
				you can upload one here. Avatar images<br />
				will be resized to 75px square.
			</div>
			<div class="push_1 grid_6">
				<input type="file" id="ProfilePictureUpload" name="ProfilePictureUpload" size="23"/>
                <%= Html.ValidationMessage("ProfilePictureUpload", "*")%>
			</div>
            <div class="grid_13 spacer-16">&nbsp;</div>
			<div class="grid_6">
				<label>About Me:</label>
			</div>
			<div class="push_1 grid_6">
                <%= Html.TextArea("AboutMe", Model.UserInformation.AboutMe, new { style = "width:300px; height: 200px" })%>
                <%= Html.ValidationMessage("AboutMe", "*")%>
			</div>
            <div class="grid_13 spacer-16">&nbsp;</div>
            <div class="grid_6">
                <label>Newsletter:</label>
            </div>
            <div class="push_1 grid_6">
                Yes <%= Html.RadioButton("Newsletter", true, Model.UserInformation.Newsletter) %>
                No <%= Html.RadioButton("Newsletter", false, !Model.UserInformation.Newsletter)%>
            </div>
            <div class="grid_13 spacer-16">&nbsp;</div>	
			<div class="prefix_9 grid_4">
                <input type="button" class="button" value="Cancel" />
			    <input type="submit" class="button" value="Save" />
            </div>
            <% } %>
        </div>

        <div class="clear">&nbsp;</div>
		<div class="grid_24 spacer-200">
			&nbsp;
		</div>
        <div class="clear">&nbsp;</div>
</asp:Content>