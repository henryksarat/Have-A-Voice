<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.ComplaintModel>" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="clear">&nbsp;</div>

	<div class="m-top30">
		<% Html.RenderPartial("Message"); %>
	
		<div class="push-1 col-5 center p-t5 p-b5 t-tab b-wht">
			<span class="fnt-16 tint-6 bold">REPORT A PROBLEM</span>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
	    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

		<div class="push-1 col-22">
		    <% using (Html.BeginForm("Complaint", "Complaint", FormMethod.Post, new { @class = "create" })) { %>
		    	<div class="normal">
		    		You are about to report a problem with <i><%= Html.Encode(NameHelper.FullName(Model.TowardUser)) %></i> against a <i><%= Html.Encode(Model.ComplaintType) %></i> they posted.
					<br /><br />
		    		<i>Summary of what they posted:</i>
		            <%= Html.Encode(Model.SourceDescription) %>
		    	</div>
		    	
		    	<div class="push-2 col-20 center stretch">
		            <%= Html.TextArea("Complaint",
		                HAVUserInformationFactory.IsLoggedIn() ? string.Empty : "You must be logged in to report a problem.", 8, 60,
		                HAVUserInformationFactory.IsLoggedIn() ?  null : new { @readonly = "readonly" }) %>
		            <%= Html.ValidationMessage("Complaint", "*") %>
		            <div class="clear">&nbsp;</div>
		            <div class="right">
		            	<input type="submit" value="Send Report" class="create" />
		            </div>
		    	</div>
	       <% } %>
			<div class="clear">&nbsp;</div>
		</div>
       <div class="clear">&nbsp;</div>
	</div>
</asp:Content>
