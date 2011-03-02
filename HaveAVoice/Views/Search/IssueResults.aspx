<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Issue>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search Issues
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">SEARCH</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
		<div class="col-24 m-btm10">
		    <% foreach (Issue myIssue in Model) { %>
		    	<div class="col-12">
		    		<div class="col-2">
		    			<img src="<%= PhotoHelper.ProfilePicture(myIssue.User)%>" alt="<%= NameHelper.FullName(myIssue.User) %>" class="profile" />
		    			<div class="clear">&nbsp;</div>
		    		</div>
		    		<div class="m-lft col-10 m-rgt">
		    			<a href="<%= myIssue.User.ShortUrl %>" class="name"><%= NameHelper.FullName(myIssue.User) %></a><br />
		    			<h4><%= myIssue.Title %></h4><br />
		    			<%= myIssue.Description %><br />
		    			<div class="right">
		    				<span class="loc">
		    					<%= myIssue.DateTimeStamp.ToString("MMM dd yyyy") %>
		    				</span>
		    			</div>
		    			<div class="clear">&nbsp;</div>
		    		</div>
		    		<div class="clear">&nbsp;</div>
		    	</div>
		    <% } %>
		    <div class="clear">&nbsp;</div>
		</div>
</asp:Content>

