<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueModel>" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers.UI" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24 m-btm30">
        <div class="spacer-30">&nbsp;</div>
        <% TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA] = TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA]; %>
        <% Dictionary<string,string> myFilter = (Dictionary<string,string>)TempData[HAVConstants.FILTER_TEMP_DATA];  %>
        <% TempData[HAVConstants.FILTER_TEMP_DATA] = myFilter; %>

    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%=Html.ActionLink("ISSUES", "Index", null, new { @class = "issue-create" }) %>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<%= Html.ActionLink("CREATE NEW", "Create", null, new { @class = "issue-create" }) %>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="push-1 col-14 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold"><%= Html.Encode(Model.Issue.Title.ToUpper()) %></span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
		<div class="push-5 col-16 m-btm10 fnt-12">
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.All, myFilter, "All", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
			</div>
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.Politicians, myFilter, "Politicians", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
			</div>
			<div class="m-lft col-4 m-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.PoliticalCandidates, myFilter, "Political Candidates", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
			</div>
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.PersonFilterButton(PersonFilter.People, myFilter, "People", "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
			</div>
		</div>
		<div class="clear">&nbsp;</div>
		
		<div class="push-7 col-9 m-btm10 fnt-12">
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.IssueStanceFilterButton(IssueStanceFilter.All, myFilter, "filter", "filterSelected") %>
                <div class="clear">&nbsp;</div>
			</div>
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.IssueStanceFilterButton(IssueStanceFilter.Agree, myFilter, "filter like", "filterSelected like") %>
                <div class="clear">&nbsp;</div>
			</div>
			<div class="m-lft col-3 m-rgt center">
                <%= IssueHelper.IssueStanceFilterButton(IssueStanceFilter.Disagree, myFilter, "filter dislike", "filterSelected dislike") %>
                <div class="clear">&nbsp;</div>
			</div>
		</div>
		<div class="clear">&nbsp;</div>

	    <% using (Html.BeginForm()) { %>
			<% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
			<% HaveAVoice.Models.User myUser = myUserInformationModel.Details; %>
	
	        <%= Html.Hidden("City", HAVUserInformationFactory.GetUserInformation().Details.City) %>
	        <%= Html.Hidden("State", HAVUserInformationFactory.GetUserInformation().Details.State) %>

	        <% Html.RenderPartial("Message"); %>
	        <%= Html.ValidationSummary("Your reply wasn't posted. Please correct the errors and try again.") %>
	        <div class="clear">&nbsp;</div>
	        
	        <div class="m-btm10">
		        <div class="push-2 col-3 center issue-profile">
					<img src="<%= PhotoHelper.ProfilePicture(Model.Issue.User) %>" alt="<%= Model.Issue.User %>" class="profile lg" />
				</div>
				<div class="push-2 m-lft col-16 m-rgt comment">
					<div class="p-a10">
						<span class="speak-lft">&nbsp;</span>
						<h1 class="m-btm10"><%= Html.Encode(Model.Issue.Title) %></h1>
						<%= Html.Encode(Model.Issue.Description) %>
		
						<div class="clear">&nbsp;</div>
						<div class="col-15 p-v10 options">
							<div class="push-6 col-3 center">
				                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue)) { %>
				                    <%= Html.ActionLink("Edit", "Edit", new { id = Model.Issue.Id }, new { @class = "edit" })%>
				                <% } else { %>
				                	&nbsp;
				                <% } %>
				                <div class="clear">&nbsp;</div>
			                </div>
			                <div class="push-6 col-3 center">
				                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue)) { %>
				                    <%= Html.ActionLink("Delete", "Delete", new { id = Model.Issue.Id }, new { @class = "delete" })%>
				                <% } else { %>
				                	&nbsp;
				                <% } %>
				                <div class="clear">&nbsp;</div>
			                </div>
			                <div class="push-6 col-3 center">
			                	<%= ComplaintHelper.IssueLink(Model.Issue.Id) %>
			                	<div class="clear">&nbsp;</div>
			                </div>
			                <div class="clear">&nbsp;</div>
		                </div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="push-2 col-3 stats fnt-12">
					<div class="p-a5">
						<h4 class="m-btm5">Stats</h4>
						<div class="bold">Posted:</div>
						<div class="m-lft10 m-btm5"><%= Model.Issue.DateTimeStamp.ToString("MMM dd, yyyy").ToUpper() %></div>
						<div class="m-btm5"><span class="bold">Likes:</span> 1</div>
						<div><span class="bold">Dislikes:</span> 1</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
	        <div class="clear">&nbsp;</div>

            <% foreach (IssueReplyModel reply in Model.Replys) { %>
                <%= IssueHelper.UserIssueReply(reply) %>

                <div class="clear">&nbsp;</div>
            <% } %>

            <% /*
            <% foreach (IssueReplyModel reply in Model.Replys) { %>
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
							<div class="clear">&nbsp;</div>
						</div>
					</div>
				</div>
			<% } else { %>
				<div class="reply">
					<div class="row">
						<div class="push-5 col-2 center">
                            <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
							<img src="<%= myUserInfo.ProfilePictureUrl %>" alt="<%= myUserInfo.FullName %>" class="profile" />
							<div class="clear">&nbsp;</div>
						</div>
						<div class="push-5 m-lft col-14 comment">
							<span class="speak-lft">&nbsp;</span>
							<div class="p-a10">
								<%= Html.TextArea("Reply", Model.Comment, 5, 63, new { resize = "none", @class = "comment" }) %>
								<span class="req">
									<%= Html.ValidationMessage("Reply", "*") %>
								</span>
								<div class="clear">&nbsp;</div>
								
							    <% if (HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Post_Anonymous_Issue_Reply)) { %>   
							        <%= Html.CheckBox("Anonymous", Model.Anonymous) %> Post reply as Anonymous
							    <% } %>
								<div class="clear">&nbsp;</div>
								<hr />
								<div class="col-13">
									<label for="Like">Like</label>
									<%= Html.RadioButton("Disposition", Disposition.Like, Model.Disposition == Disposition.Like ? true : false) %>
									<label for="Dislike">Dislike</label>
									<%= Html.RadioButton("Disposition", Disposition.Dislike, Model.Disposition == Disposition.Dislike ? true : false) %>
									<span class="req">
										<%= Html.ValidationMessage("Disposition", "*")%>
									</span>
									<div class="clear">&nbsp;</div>
								</div>
								<div class="clear">&nbsp;</div>
								<hr />
								<div class="col-13">
									<input type="submit" value="Submit" class="button" />
									<input type="button" value="Clear" class="button" />
									<div class="clear">&nbsp;</div>
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