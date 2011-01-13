<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.View.IssueWithDispositionModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Issues
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<%= Html.Encode(ViewData["Message"]) %>
    	<%= Html.Encode(TempData["Message"]) %>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">ISSUES</span>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%= Html.ActionLink("CREATE NEW", "Create", null, new { @class = "issue-create" }) %>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>

	    <% foreach (var item in Model) { %>
	    	<div class="col-3">
	    		<img src="/Photos/93_-943723973.jpg" alt="hsarat" class="profile" />
	    	</div>
	    	<div class="col-18 issue">
	    		<div class="p-a5">
		    		<h1><%= Html.ActionLink(item.Issue.Title, "View", new { id = item.Issue.Id })%></h1>
					<br />
		    		<%= item.Issue.Description %>
	    		</div>
	    		<div class="clear">&nbsp;</div>
	    		<div class="col-9">&nbsp;</div>
	    		
	            <% if (item.Issue.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id) { %>
	            	<div class="col-3 center">
	            		<%= Html.ActionLink("Delete", "DeleteIssue", new { deletingUserId = HAVUserInformationFactory.GetUserInformation().Details.Id, issueId = item.Issue.Id}, new { @class = "delete" }) %>
	            	</div>
	            <% } else { %>
	            	<div class="col-3">&nbsp;</div>
	            <% } %>
	            
	            <% if (!item.HasDisposition) { %>
	            	<div class="col-3 center">
	            		<%= Html.ActionLink("Like", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.LIKE }, new { @class = "like" })%>
	            	</div>
	                <div class="col-3 center">
	                	<%= Html.ActionLink("Dislike", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.DISLIKE }, new { @class = "dislike" })%>
	                </div>
	            <% } else { %>
	            	<div class="col-6">&nbsp;</div>
	            <% } %>
	    	</div>
			
			<div class="col-3 right">
				<div class="p-a5">
					<div class="date-tile">
						<span>
							<%= DateHelper.ConvertToLocalTime(item.Issue.DateTimeStamp).ToString("MMM").ToUpper() %>
						</span> <%= DateHelper.ConvertToLocalTime(item.Issue.DateTimeStamp).ToString("dd").ToUpper() %>
					</div>
				</div>
			</div>
	    
	    	<div class="clear">&nbsp;</div>
	    	<div class="spacer-10">&nbsp;</div>
	    <% } %>
	</div>
</asp:Content>

