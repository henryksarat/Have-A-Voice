<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditPrivacySettingsModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Privacy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-24">
	    <div class="col-6 profile">
	        <img src="<%= Model.ProfilePictureURL %>" alt="<%= Model.UserInformation.Username %>" class="profile" />
	        <br />
	        <div class="col-6 p-v10 details">
				<span class="blue">Name:</span> <%= Model.UserInformation.FirstName + " " + Model.UserInformation.LastName %><br />
				<span class="blue">Gender:</span> GENDER<br />
				<span class="blue">Site:</span> <%= Model.UserInformation.Website %><br />
	            <span class="blue">Email:</span> <%= Model.UserInformation.Email %><br />
			</div>

	        <div class="col-6">
			    <h4>About Me</h4>
	            <%= Model.UserInformation.AboutMe %>
	        </div>
	    </div>
	    
		<div class="m-lft col-18 form">
		    <% Html.RenderPartial("Message"); %>

			<% Html.RenderPartial("PrivacyTabs", UserSettings.AccountSettings); %>

			<div class="clear">&nbsp;</div>

		    <% using (Html.BeginForm()) { %>
		    	<% foreach (HAVPrivacySetting mySetting in Enum.GetValues(typeof(HAVPrivacySetting))) { %>
		            <div class="col-6">
		                <label><%= mySetting.ToSTring() %>:</label>
		            </div>
		            <div class="push-1 col-6">
		                Yes <%= Html.RadioButton(mySetting.ToString(), true, Model.PrivacySettings[mySetting.ToString()].Second) %>
		                No <%= Html.RadioButton(mySetting.ToString(), false, !Model.PrivacySettings[mySetting.ToString()].Second) %>
		            </div>
		            <div class="col-18 spacer-15">&nbsp;</div>
		    	<% } %>
                <input type="button" class="button" value="Cancel" />
			    <input type="submit" class="button" value="Save" />
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
