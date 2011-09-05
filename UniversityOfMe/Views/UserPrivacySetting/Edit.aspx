<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<DisplayPrivacySettingsModel<PrivacySetting>>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.Generic" %>
<%@ Import Namespace="Social.BaseWebsite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Edit Privacy Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form">
		    <div class="banner title black full small"> 
			    <span class="edit">EDIT YOUR PRIVACY SETTINGS</span> 
            </div>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
		        <% using (Html.BeginForm()) { %>
		            <% foreach (string myGroup in Model.Get().PrivacySettings.Keys) { %>
                        <div class="underline-heading bold"><%= myGroup %></div>
                        <% foreach(Pair<PrivacySetting, bool> myPair in Model.Get().PrivacySettings[myGroup]) { %>
		    	        <div class="ml20 wp100" style="display:inline-block">		    		
			                <div class="flft">
			                    <div class="bold"><%= myPair.First.DisplayName%></div>
                                <div class="small mt-6"><%= myPair.First.Description %></div>
			                </div>
			                <div class="frgt mr26 small">
			                    Yes <%= Html.RadioButton(myPair.First.Name, true, myPair.Second)%>
			                    No <%= Html.RadioButton(myPair.First.Name, false, !myPair.Second)%>
			                </div>
		                </div>
                        <% } %>
		            <% } %>
		            <div class="right"> 
				        <input type="submit" class="btn site" value="Save" />
		            </div>
		        <% } %>
            </div>
		    <div class="clear">&nbsp;</div>
        </div>
	</div>
	<div class="clear">&nbsp;</div>

</asp:Content>
