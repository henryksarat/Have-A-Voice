﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DisplayPrivacySettingsModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="Social.Generic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Privacy Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-24">
	    <div class="col-6 profile m-top30">
	        <img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.FullName %>" class="profile" />
	        <br />
	        <div class="col-6 p-v10 details">
				<span class="blue">Name:</span> <%= NameHelper.FullName(Model.NavigationModel.User)%><br />
				<span class="blue">Gender:</span> <%= Model.NavigationModel.User.Gender %><br />
				<span class="blue">Site:</span> <%= Model.NavigationModel.User.Website %><br />
	            <span class="blue">Email:</span> <%= Model.NavigationModel.User.Email %><br />
			</div>

	        <div class="col-6 details">
			    <h4>About Me</h4>
	            <%= Model.NavigationModel.User.AboutMe %>
	        </div>
	    </div>
	    
		<div class="m-lft col-18 form">
		    <% Html.RenderPartial("Message"); %>

			<% Html.RenderPartial("PrivacyTabs", UserSettings.PrivacySettings); %>

			<div class="clear">&nbsp;</div>

		    <% using (Html.BeginForm()) { %>
		    	<% foreach (string myGroup in Model.PrivacySettings.Keys) { %>
                    <h4><%= myGroup %></h4>
                    <div class="clear">&nbsp;</div>
                    <% foreach(Pair<PrivacySetting, bool> myPair in Model.PrivacySettings[myGroup]) { %>
		    		<div class="push-1 col-13 m-btm15">		    		
			            <div class="col-6">
			                <label><%= myPair.First.DisplayName%></label>
                            <%= myPair.First.Description %>
			            </div>
			            <div class="push-1 col-6">
			                <%= Html.RadioButton(myPair.First.Name, true, myPair.Second)%> Yes
			                <%= Html.RadioButton(myPair.First.Name, false, !myPair.Second)%> No
			            </div>
			            <div class="clear">&nbsp;</div>
		            </div>
		            <div class="clear">&nbsp;</div>
                    <% } %>
		    	<% } %>
		    	<div class="push-7 col-4">
					<input type="submit" class="button" value="Save" />
					<input type="button" class="button" value="Cancel" />
					<div class="clear">&nbsp;</div>
		    	</div>
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
