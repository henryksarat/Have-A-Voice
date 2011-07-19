<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Group>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="col-24 m-btm30">
	<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

    <div class="spacer-30">&nbsp;</div>

	<% Html.RenderPartial("Message"); %>
	<% Html.RenderPartial("Validation"); %>
	<div class="clear">&nbsp;</div>

    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
        <a class="issue-create" href="/Group/List">Groups</a>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%= Html.ActionLink("CREATE GROUP", "Create", "Group", null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-14 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold"><%= Html.Encode(Model.Name.ToUpper()) %></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>
		
    <div class="push-21 alpha col-2 omega">
		<div class="p-a5">
            <a href="http://twitter.com/share" class="twitter-share-button" data-count="none" data-via="haveavoice_">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>
        </div>
        <div class="clear">&nbsp;</div>
    </div>

    <div class="push-21 alpha col-2">
		<div class="p-a5">
            <script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like href="#" layout="button_count" show_faces="false" width="90" font="arial"></fb:like>
        </div>
        <div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>

	<% using (Html.BeginForm("Create", "GroupBoard", FormMethod.Post, new { @class = "create" })) { %>
	    <div class="clear">&nbsp;</div>
	        
	    <div class="m-btm10">
            <div class="push-2 m-lft col-16 m-rgt comment">                <div class="p-a10">                    <h1><a href="/Issue/Details/test">test</a></h1>                    </br>ttttttt                    <div class="clear">&nbsp;                    </div>                    <div class="col-15 p-v10">                        <div class="col-3">                            Tags:<br />                             <%= string.Join(",", Model.GroupTags.Select(t => t.Tag)) %>                        </div>                        <div class="clear">&nbsp;</div>                    </div>                </div>            </div>
            <div class="push-2 col-3 stats fnt-12 m-rgt">
                <div class="clear">&nbsp;</div>
                <div class="p-a5">
                    <h4 class="m-btm5">Group Stats</h4>
                    <div class="bold">Created:</div>
                    <div class="m-lft10 m-btm5"><%= LocalDateHelper.ToLocalTime(Model.DateTimeStamp) %></div>
                    <% int myGroupMembers = Model.GroupMembers.Where(gm => !gm.Deleted).Where(gm => gm.Approved == HAVConstants.APPROVED).Count<GroupMember>(); %>
                    <div class="m-btm5"><span class="bold">Members: </span><%= myGroupMembers %></div>
                    <div class=""><span class="bold">Board Posts: </span><%= Model.GroupBoards.Count %></div>
                </div>
            </div>

            <div class="push-2 col-3 group fnt-12">
                <div class="admin-feed">
                    <div class="clear">&nbsp;</div>
                    <div class="p-a5">
                        <h4 class="m-btm5">Group Options</h4>
                        <% if (GroupHelper.IsAdmin(myUserInfo.Details, Model.Id)) { %>
                            <%= Html.ActionLink("Edit", "Edit", "Group", new { id = Model.Id }, new { @class = "grouplink" })%><br />
                            <% if (Model.Active) { %>                
                                <%= Html.ActionLink("Set club as inactive", "Deactivate", "Group", new { id = Model.Id }, new { @class = "grouplink" })%>
                            <% } else { %>
                                <%= Html.ActionLink("Set club as active", "Activate", "Group", new { id = Model.Id }, new { @class = "grouplink" })%>
                            <% } %>    
                            <% } else if (GroupHelper.IsPending(myUserInfo.Details, Model.Id)) { %>
                                <%= Html.ActionLink("Cancel my request to join", "Cancel", "GroupMember", new { groupId = Model.Id }, new { @class = "grouplink" })%>
                            <% } else if (GroupHelper.IsMember(myUserInfo.Details, Model.Id)) { %>
                                <%= Html.ActionLink("Quit organization", "Remove", "GroupMember", new { groupId = Model.Id }, new { @class = "grouplink" })%>
                            <% } else { %>
                                <%= Html.ActionLink("Request to join organization", "RequestToJoin", "GroupMember", new { groupId = Model.Id }, new { @class = "grouplink" })%>
                        <% } %>
                        <a class="grouplink" href="/GroupMember/List/<%= Model.Id %>">View all members</a>
                    </div>
                </div>
            </div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>

        <div class="group">
            <div class="push-6 m-lft col-14 m-rgt header">    
                <div class="p-a10">
                    Admin Feed
                </div>
            </div>
            <% foreach (GroupAdminFeed myFeed in GroupHelper.GetAdminFeed(Model, 5)) { %>
                <div class="push-6 m-lft col-14 m-rgt admin-feed">
                    <div class="p-a10">
                        <% if (myFeed.FeedType == FeedType.Member) { %>
                            <% if (myFeed.Status == Status.Pending) { %>
                                <a class="grouplink" href="<%= LinkHelper.Profile(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser)%></a> 
                                wants to join this organization. 
                                <%= Html.ActionLink("Approve/Deny", "Details", "GroupMember", new { groupId = myFeed.GroupId, groupMemberId = myFeed.GroupMemberId }, new { @class = "grouplink" })%>
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                            <% } else if(myFeed.Status == Status.Denied) { %>
                                <a class="grouplink" href="<%= LinkHelper.Profile(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                has denied 
                                <a class="grouplink" href="<%= LinkHelper.Profile(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser) %></a> 
                                to join the organization.
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                            <% } else if (myFeed.Status == Status.Approved) { %>
                                <a class="grouplink" href="<%= LinkHelper.Profile(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser)%></a> 
                                has approved 
                                <a class="grouplink" class=grouplink" href="<%= LinkHelper.Profile(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser)%></a> 
                                to join the organization.
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                            <% } %>
                        <% } else if(myFeed.FeedType == FeedType.Edited) { %>
                                <a class="grouplink" class=grouplink" href="<%= LinkHelper.Profile(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                has updated the club details. 
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                        <% } else if(myFeed.FeedType == FeedType.Deactivated) { %>
                                <a class="grouplink" class=grouplink" href="<%= LinkHelper.Profile(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                has deactivated the club.
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                        <% } else if(myFeed.FeedType == FeedType.AutoAcceptedMember) { %>
                                <a class="grouplink" href="<%= LinkHelper.Profile(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser)%></a> 
                                joined the group.
                                <span class="admin-feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                        <% } %>
                    </div>
                </div>
            <% } %>
        </div>

        <div class="clear">&nbsp;</div>

        <div class="group m-top10">
            <div class="push-6 m-lft col-14 m-rgt header">    
                <div class="p-a10">
                    Group Board
                </div>
            </div>
        </div>

	    <div class="clear">&nbsp;</div>
        <%= Html.Hidden("GroupId", Model.Id) %>
        <% string myProfilePicture = Social.Generic.Constants.Constants.ANONYMOUS_PICTURE_URL; %>
        <% string myFullName = string.Empty; %>
        <% bool myIsLoggedIn = HAVUserInformationFactory.IsLoggedIn();  %>

		<div class="reply m-btm10 m-top10">
			<div class="row">
				<div class="push-5 col-2 center">
					<img src="<%= myProfilePicture %>" alt="<%= myFullName %>" class="profile" />
					<div class="clear">&nbsp;</div>
				</div>
				<div class="push-5 m-lft col-14 comment">

					<span class="speak-lft">&nbsp;</span>

					<div class="p-a10">
						<%= Html.TextArea("BoardMessage", string.Empty, 3, 63, new { resize = "none", @class = "comment" })%>
						<span class="req">
							<%= Html.ValidationMessage("BoardMessage", "*")%>
						</span>
								
						<div class="clear">&nbsp;</div>
						<div class="col-13">
							<input type="submit" value="Submit" class="button" />
							<div class="clear">&nbsp;</div>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
        </div>
        <div class="clear">&nbsp;</div>
        <% foreach (GroupBoard myGroupBoard in Model.GroupBoards.OrderByDescending(gb => gb.DateTimeStamp)) { %>
            <div class="group m-btm10">                <div class="push-6 col-2 center">                    <a href="/hsarat"><img alt="Jim Bob" class="profile" src="/Photos/no_profile_picture.jpg"></img></a>                    <div class="clear">&nbsp;</div>                </div>                <div class="push-6 m-lft col-12 m-rgt comment">                    <span class="speak-lft">&nbsp;</span>                    <div class="p-a10">                        <a class="name" href="/hsarat">Jim Bob</a>&nbsp;<%= myGroupBoard.Message %>                        <div class="clear">&nbsp;</div>                        <div class="col-11 options">                            <div class="p-v10">                                <div class="col-3 center">&nbsp;</div>                                <div class="col-2 center">&nbsp;</div>                                <div class="clear">&nbsp;</div>                            </div>                        </div>                    </div>                </div>                <div class="col-3 date-tile push-6">                    <div class="p-a10">                        <div class="">                            <span>JUL</span>&nbsp;18                        </div>                    </div>                    <div class="clear">&nbsp;</div>                </div>                <div class="clear">&nbsp;</div>            </div>
            <div class="clear">&nbsp;</div>
        <% } %>
	<% } %>
</div>
</asp:Content>