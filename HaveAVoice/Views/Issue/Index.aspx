<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.View.IssueWithDispositionModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Issues
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="col-24 m-btm10 right">
    		<a href="/Issue/Create" class="button">Raise New Issue</a>
    		<div class="clear">&nbsp;</div>
    	</div>
    
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
		    	<div class="push-1 col-17 m-lft issue m-rgt">
		    		<div class="p-a5">
			    		<h1><a href="<%= LinkHelper.IssueUrl(item.Issue.Title) %>"><%= item.Issue.Title %></a></h1>
						<br />
			    		<%= item.Issue.Description %>
			    		<br />
                        <% string myName = NameHelper.FullName(item.Issue.User); %>
                        <% string myIssueProfile = LinkHelper.Profile(item.Issue.User); %>
                        <br />
						Raised by <a class="name-2" href="<%= myIssueProfile %>"><%= myName %></a>
                        From
			    		<span class="loc"><%= item.Issue.User.City %>, <%= item.Issue.User.State %></span>
			    		<div class="clear">&nbsp;</div>
		    		</div>
		    		
                    <div class="p-a5">
			            <% if (false) { %>
			            	<div class="push-4 col-3 center">
			            		<%= Html.ActionLink("Delete", "DeleteIssue", new { deletingUserId = HAVUserInformationFactory.GetUserInformation().Details.Id, issueId = item.Issue.Id}, new { @class = "delete" }) %>
			            		<div class="clear">&nbsp;</div>
			            	</div>
			            <% } else { %>
			            	<div class="push-4 col-3">&nbsp;</div>
			            <% } %>
			            
			            <div class="push-4 col-3 center">
			            	<span class="color-1">
			            		<% if(item.Issue.IssueReplys.Count == 1) { %> 
                                    <%= item.Issue.IssueReplys.Count %> Reply
                                <%} else { %>
                                    <%= item.Issue.IssueReplys.Count %> Replies
                                <% } %>
			            	</span>
			            </div>
			            
			            <% if (!item.HasDisposition) { %>
			            	<div class="push-4 col-3 center">
			            		<a href="<%= LinkHelper.AgreeIssue(item.Issue.Id, SiteSection.Issue, item.Issue.Id) %>" class="like">Agree (<%= item.TotalAgrees %>)</a>
			            		<div class="clear">&nbsp;</div>
			            	</div>
			                <div class="push-4 col-3 center">
			                	<a href="<%= LinkHelper.AgreeIssue(item.Issue.Id, SiteSection.Issue, item.Issue.Id) %>" class="dislike">Disagree (<%= item.TotalDisagrees %>)</a>
			                	<div class="clear">&nbsp;</div>
			                </div>
			            <% } else { %>
			            	<div class="push-4 col-3 center">
								<span class="like">
									<%= item.TotalAgrees %>
									<% if (item.TotalAgrees == 1) { %>
										Person Agrees
									<% } else { %>
										People Agree
									<% } %>
								</span>
	                            <div class="clear">&nbsp;</div>
	                        </div>
	                        <div class="push-4 col-3 center">
								<span class="dislike">
									<%= item.TotalDisagrees%>
									<% if (item.TotalDisagrees == 1) { %>
										Person Disagrees
									<% } else { %>
										People Disagree 
									<% } %>
								</span>
	                            <div class="clear">&nbsp;</div>
	                        </div>
			            <% } %>
	                    <div class="clear">&nbsp;</div>
			        </div>
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