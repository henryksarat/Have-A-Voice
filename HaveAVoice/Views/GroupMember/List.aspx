<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<GroupMember>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Group
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
    	<span class="fnt-16 tint-6 bold"><%= Html.Encode(Model.FirstOrDefault<GroupMember>().Group.Name.ToUpper() + " MEMBER LIST") %></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>

    <div class="clear">&nbsp;</div>
	<div class="col-21 m-btm10">
    	<div class="col-15 m-lft m-rgt fnt-18 bold color-1">
 	        <% foreach (GroupMember myGroupMember in Model) { %>
	            <div class="col-21 m-btm10">
	    	        <div class="col-2 center">
                        <a href="<%= LinkHelper.Profile(myUserInfo.Details) %>">
	    		            <img src="<%= PhotoHelper.ProfilePicture(myGroupMember.MemberUser) %>" class="profile sm" />
                        </a>
	    		        <div class="clear">&nbsp;</div>
	    	        </div>
    		        <div class="col-15 m-lft m-rgt fnt-12 bold color-1">
                        <a class="name" href="<%= LinkHelper.Profile(myUserInfo.Details) %>">
    			            <%= HaveAVoice.Helpers.NameHelper.FullName(myGroupMember.MemberUser)%>
                        </a>
    			        <div class="clear">&nbsp;</div>
    		        </div>
                    <% if (GroupHelper.IsAdmin(myUserInfo, myGroupMember.GroupId)) { %>
    		            <div class="col-2 center">
    			            <%= Html.ActionLink("Edit", "Details", "GroupMember", new { groupMemberId = myGroupMember.Id, groupId = myGroupMember.GroupId }, new { @class = "button" })%>
    			            <div class="clear">&nbsp;</div>
    		            </div>
    		            <div class="col-2 center">
    			            <%= Html.ActionLink("Delete", "Remove", "GroupMember", new { groupId = myGroupMember.GroupId, userRemovingId = myGroupMember.MemberUserId }, new { @class = "button" })%>
    			            <div class="clear">&nbsp;</div>
    		            </div>
                    <% } %>
    		        <div class="clear">&nbsp;</div>
    	        </div>
	        <% } %>
        </div>
    	<div class="clear">&nbsp;</div>
    </div>
	<div class="clear">&nbsp;</div>
</div>
</asp:Content>