<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueModel>" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers.UI" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Issue - <%= Model.Issue.Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24 m-btm30">
        <div class="spacer-30">&nbsp;</div>
        <% TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA] = TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA]; %>
        <% Dictionary<string,string> myFilter = (Dictionary<string,string>)TempData[HAVConstants.FILTER_TEMP_DATA];  %>
        <% TempData[HAVConstants.FILTER_TEMP_DATA] = myFilter; %>

    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
            <a class="issue-create" href="/Issue/Index">ISSUES</a>
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

        <div class="push-21 alpha col-2 omega">
			<div class="p-a5">
                <a href="http://twitter.com/share" class="twitter-share-button" data-count="none" data-via="haveavoice_">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>
           </div>
           <div class="clear">&nbsp;</div>
        </div>

        <div class="push-21 alpha col-2">
			<div class="p-a5">
                <script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like href="<%= HAVConstants.BASE_URL + LinkHelper.IssueUrl(Model.Issue.Title) %>" layout="button_count" show_faces="false" width="90" font="arial"></fb:like>
           </div>
           <div class="clear">&nbsp;</div>
        </div>
        <div class="clear">&nbsp;</div>

	    <% using (Html.BeginForm()) { %>
            
			<% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
	        <% if (myUserInformationModel != null) { %>
                <%= Html.Hidden("City", myUserInformationModel.Details.City)%>
	            <%= Html.Hidden("State", myUserInformationModel.Details.State)%>
            <% } %>
            <%= Html.Hidden("IssueId", Model.Issue.Id)%>
            <%= Html.Hidden("TotalAgrees", Model.TotalAgrees)%>
            <%= Html.Hidden("TotalDisagrees", Model.TotalDisagrees)%>

	        <% Html.RenderPartial("Message"); %>
	        <% Html.RenderPartial("Validation"); %>
	        <div class="clear">&nbsp;</div>
	        
	        <div class="m-btm10">
                <%= SharedContentStyleHelper.ProfilePictureDiv(Model.Issue.User, false, "push-2 col-3 center issue-profile", "profile lg")%>
                <%= IssueHelper.IssueInformationDiv(Model.Issue, "push-2 m-lft col-16 m-rgt comment", "col-15 p-v10 options", "push-6 col-3 center", "push-6 col-3 center", "push-6 col-3 center")%>
                <%= IssueHelper.IssueStats(Model.Issue, "push-2 col-3 stats fnt-12", "p-a5", "h4", "m-btm5", "bold", "m-lft10 m-btm5", "m-btm5", string.Empty, "bold", "MMM dd, yyyy") %>
				<div class="clear">&nbsp;</div>
			</div>
	        <div class="clear">&nbsp;</div>

            <% foreach (IssueReplyModel reply in Model.Replys) { %>
                <%= IssueReplyHelper.UserIssueReply(reply) %>

                <div class="clear">&nbsp;</div>
            <% } %>

			<% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
				<div class="reply">
					<div class="row">
						<div class="m-lft col-20 comment push-4">
							<div class="msg-2">
								You must be logged in to reply.
							</div>
							<div class="clear">&nbsp;</div>
						</div>
					</div>
				</div>
			<% } else { %>
				<div class="reply">
					<div class="row">
						<div class="push-5 col-2 center">
							<img src="<%= myUserInformationModel.ProfilePictureUrl %>" alt="<%= myUserInformationModel.FullName %>" class="profile" />
							<div class="clear">&nbsp;</div>
						</div>
						<div class="push-5 m-lft col-14 comment">
							<span class="speak-lft">&nbsp;</span>
							<div class="p-a10">
								<%= Html.TextArea("Reply", Model.Comment, 5, 63, new { resize = "none", @class = "comment" })%>
								<span class="req">
									<%= Html.ValidationMessage("Reply", "*")%>
								</span>
								<div class="clear">&nbsp;</div>
								
							    <% if (HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Post_Anonymous_Issue_Reply)) { %>   
							        <%= Html.CheckBox("Anonymous", Model.Anonymous)%> Post reply as Anonymous
							    <% } %>
								<div class="clear">&nbsp;</div>
								<hr />
								<div class="col-13">
									<label for="Like">Agree</label>
									<%= Html.RadioButton("Disposition", Disposition.Like, Model.Disposition == Disposition.Like ? true : false)%>
									<label for="Dislike">Disagee</label>
									<%= Html.RadioButton("Disposition", Disposition.Dislike, Model.Disposition == Disposition.Dislike ? true : false)%>
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
						<div</div> class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
			<% } %>
	    <% } %>
    </div>
</asp:Content>