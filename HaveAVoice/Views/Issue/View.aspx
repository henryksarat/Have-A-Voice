<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueModel>" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers.UI" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24 m-btm30">
        <div class="spacer-30">&nbsp;</div>
    
    	<%= Html.Encode(ViewData["Message"]) %>
    	<%= Html.Encode(TempData["Message"]) %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("ISSUES", "Index", null, new { @class = "issue-create" }) %>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%= Html.ActionLink("CREATE NEW", "Create", null, new { @class = "issue-create" }) %>
    	</div>
    	<div class="push-1 col-14 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold"><%= Html.Encode(Model.Issue.Title.ToUpper()) %></span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
		<div class="push-7 col-9 m-btm10">
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter">ALL</a>
			</div>
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter">Politicians</a>
			</div>
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter">People</a>
			</div>
		</div>
		<div class="clear">&nbsp;</div>
		
		<div class="push-7 col-9 m-btm10">
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter">ALL</a>
			</div>
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter like">Agree</a>
			</div>
			<div class="m-lft col-3 m-rgt center">
				<a href="#" class="filter dislike">Disagree</a>
			</div>
		</div>
		<div class="clear">&nbsp;</div>

	    <% using (Html.BeginForm()) { %>
			<% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
			<% HaveAVoice.Models.User myUser = myUserInformationModel.Details; %>
	
	        <%= Html.Hidden("City", HAVUserInformationFactory.GetUserInformation().Details.City) %>
	        <%= Html.Hidden("State", HAVUserInformationFactory.GetUserInformation().Details.State) %>
	            
			<%= Html.Encode(ViewData["Message"]) %>
	        <%= Html.Encode(TempData["Message"]) %>
	        <%= Html.ValidationSummary("Your reply wasn't posted. Please correct the errors and try again.") %>
	        <div class="clear">&nbsp;</div>
	        
	        
	        <div class="push-2 col-3 center issue-profile">
				<img src="http://www2.pictures.zimbio.com/img/5519/Alicia/5986c.nZBops.jpg" alt="Gerard Butler" class="profile" />
			</div>
			<div class="push-2 m-lft col-16 m-rgt comment">
				<div class="p-a10">
					<span class="speak-lft">&nbsp;</span>
					<h1 class="m-btm10"><%= Html.Encode(Model.Issue.Title) %></h1>
					<%= Html.Encode(Model.Issue.Description) %>
	
					<div class="clear">&nbsp;</div>
					<div class="push-4 col-3">
		                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue)) { %>
		                    <%= Html.ActionLink("Edit", "Edit", new { id = Model.Issue.Id })%>
		                <% } %>
	                </div>
	                <div class="push-4 col-3">
		                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue)) { %>
		                    <%= Html.ActionLink("Delete", "Delete", new { id = Model.Issue.Id })%>
		                <% } %>
	                </div>
	                <div class="push-4 col-3">
	                	<%= ComplaintHelper.IssueLink(Model.Issue.Id) %>
	                </div>
				</div>
			</div>
			<div class="push-2 col-3 stats">
				<div class="p-a5">
					<h4 class="m-btm5">Stats</h4>
					<div class="bold">Posted:</div>
					<div class="m-lft10 m-btm5"><%= Model.Issue.DateTimeStamp.ToString("MMM dd, yyyy").ToUpper() %></div>
					<div class="bold">Likes:</div>
					<div class="m-lft10 m-btm5">1</div>
					<div class="bold">Dislikes:</div>
					<div class="m-lft10 m-btm5">1</div>
				</div>
			</div>
	        

            <% foreach (IssueReplyModel reply in Model.UserReplys) { %>
                <%= IssueHelper.UserIssueReply(reply) %>
                
                <!-- IS THERE ANY WAY TO MOVE THIS INTO THE UserIssueReply() function? //-->
                <% if (!reply.HasDisposition) { %>
                        <%= Html.ActionLink("Like", "Disposition", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id, disposition = (int)Disposition.Like }, null)%>
                        <%= Html.ActionLink("Dislike", "Disposition", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id, disposition = (int)Disposition.Dislike }, null)%>
                <% } %>
                <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue_Reply)) { %>
                    <%= Html.ActionLink("Edit", "Edit", "IssueReply", new { id = reply.Id }, null)%>
				<% } %>
				<% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue_Reply)) { %>
					<%= Html.ActionLink("Delete", "Delete", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id }, null)%>
                <% } %>
                <div class="clear">&nbsp;</div>
            <% } %>

<% /*
<% foreach (IssueReplyModel reply in Model.OfficialReplys) { %>
    <p>
        <%= IssueHelper.OfficialIssueReply(reply) %>
    </p>
    <p>
        <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue_Reply)) { %>
            <%= Html.ActionLink("Edit", "Edit", "IssueReply", new { id = reply.Id }, null)%>
        <% } %>
       <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue_Reply)) { %>
            <%= Html.ActionLink("Delete", "Delete", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id }, null)%>
        <% } %>
    </p>
<%}%>
<% */ %>

			<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
				<div class="reply">
					<div class="row">
						<div class="m-lft col-14 comment push-4">
							You must be logged in to reply.
						</div>
					</div>
				</div>
			<% } else { %>
				<div class="reply">
					<div class="row">
						<div class="push-2 col-2 center">
							<img src="http://www2.pictures.zimbio.com/img/5519/Alicia/5986c.nZBops.jpg" alt="Gerard Butler" class="profile" />
						</div>
						<div class="push-2 m-lft col-14 comment">
							<span class="speak-lft">&nbsp;</span>
							<div class="p-a10">
								<%= Html.ValidationMessage("Reply", "*") %>
								<%= Html.TextArea("Reply", Model.Comment, 5, 63, new { resize = "none", @class = "comment" }) %>
								<div class="clear">&nbsp;</div>
							    <% if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Official_Account)) { %>   
							        <%= Html.CheckBox("Anonymous", Model.Anonymous) %> Post reply as Anonymous
							    <% } %>
								<div class="clear">&nbsp;</div>
								<hr />
								<div class="col-13">
									<%= Html.ValidationMessage("Disposition", "*")%>
									<div class="clear">&nbsp;</div>
									<label for="Like">Like</label>
									<%= Html.RadioButton("Disposition", Disposition.Like, Model.Disposition == Disposition.Like ? true : false) %>
									<label for="Dislike">Dislike</label>
									<%= Html.RadioButton("Disposition", Disposition.Dislike, Model.Disposition == Disposition.Dislike ? true : false) %>
								</div>
								<div class="clear">&nbsp;</div>
								<hr />
								<div class="col-13">
									<input type="submit" value="Submit" class="button" />
									<input type="button" value="Clear" class="button" />
								</div>
								<div class="clear">&nbsp;</div>
							</div>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
			<% } %>
	    <% } %>
    </div>
</asp:Content>