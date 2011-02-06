<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.View.IssueWithDispositionModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Issues
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">ISSUES</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%= Html.ActionLink("CREATE NEW", "Create", null, new { @class = "issue-create" }) %>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>

	    <% foreach (var item in Model) { %>
	    	<div class="issue-container m-btm10">
		    	<div class="push-1 col-2">
		    		<img src="<%= PhotoHelper.ProfilePicture(item.Issue.User) %>" alt="<%= NameHelper.FullName(item.Issue.User) %>" class="profile" />
		    		<div class="clear">&nbsp;</div>
		    	</div>
		    	<div class="push-1 col-17 issue">
		    		<div class="p-a5">
			    		<h1><%= Html.ActionLink(item.Issue.Title, "View", new { id = item.Issue.Id })%></h1>
						<br />
			    		<%= item.Issue.Description 
			    		<div class="clear">&nbsp;</div>%>
		    		</div>
		    		<div class="clear">&nbsp;</div>
		    		
		            <% if (item.Issue.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id) { %>
		            	<div class="push-7 col-3 center">
		            		<%= Html.ActionLink("Delete", "DeleteIssue", new { deletingUserId = HAVUserInformationFactory.GetUserInformation().Details.Id, issueId = item.Issue.Id}, new { @class = "delete" }) %>
		            		<div class="clear">&nbsp;</div>
		            	</div>
		            <% } else { %>
		            	<div class="push-7 col-3">&nbsp;</div>
		            <% } %>
		            
		            <% if (!item.HasDisposition) { %>
		            	<div class="push-7 col-3 center">
		            		<%= Html.ActionLink("Agree", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.Like }, new { @class = "like" }) %>
		            		<div class="clear">&nbsp;</div>
		            	</div>
		                <div class="push-7 col-3 center">
		                	<%= Html.ActionLink("Disagree", "Disposition", new { issueId = item.Issue.Id, disposition = (int)Disposition.Dislike }, new { @class = "dislike" }) %>
		                	<div class="clear">&nbsp;</div>
		                </div>
		            <% } else { %>
		            	<div class="push-7 col-6">&nbsp;</div>
		            <% } %>
		    	</div>
				
				<div class="push-1 col-3">
					<div class="p-a5">
						<div class="date-tile">
							<span>
								<%= DateHelper.ConvertToLocalTime(item.Issue.DateTimeStamp).ToString("MMM").ToUpper() %>
							</span> <%= DateHelper.ConvertToLocalTime(item.Issue.DateTimeStamp).ToString("dd").ToUpper() %>
						</div>
					</div>
				</div>
		    
		    	<div class="clear">&nbsp;</div>
			</div>
	    <% } %>
	</div>
</asp:Content>