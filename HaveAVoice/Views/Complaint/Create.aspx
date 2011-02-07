<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.ComplaintModel>" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="clear">&nbsp;</div>

	<div class="m-top30">
		<div class="push-1 col-5 center p-t5 p-b5 t-tab b-wht">
			<span class="fnt-16 tint-6 bold">REPORT A PROBLEM</span>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>

		<% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
	    <div class="clear">&nbsp;</div>

		<div class="push-1 col-22">
		    <% using (Html.BeginForm("Create", "Complaint", FormMethod.Post, new { @class = "create" })) { %>
                <%= Html.Hidden("ComplaintType", Model.ComplaintType) %>
                <%= Html.Hidden("SourceId", Model.SourceId) %>
		    	<div class="normal">
		    		You are about to report a problem with <i><%= Html.Encode(NameHelper.FullName(Model.TowardUser)) %></i> against a <i><%= Html.Encode(Model.ComplaintType) %></i> they posted.
					<br /><br />
		    		<i>Summary of what they posted:</i>
                    <% if (Model.ComplaintType == HaveAVoice.Helpers.Enums.ComplaintType.PhotoComplaint) { %>
                        <img src="<%= HaveAVoice.Services.Helpers.PhotoHelper.ConstructUrl(Model.SourceDescription) %>" />
                    <% } else { %>
		                <%= Html.Encode(Model.SourceDescription) %>
                    <%} %>
		    	</div>
		    	
		    	<div class="push-2 col-20 center stretch">
		            <%= Html.TextArea("Complaint", (string)TempData["ComplaintBody"], 8, 60, null) %>
		            <span class="req">
		            	<%= Html.ValidationMessage("Complaint", "*") %>
		            </span>
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
