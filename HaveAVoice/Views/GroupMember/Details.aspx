<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupMember>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="col-24 m-btm30">
    <div class="spacer-30">&nbsp;</div>

	<% Html.RenderPartial("Message"); %>
	<% Html.RenderPartial("Validation"); %>

    <% bool myClubMemberApproval = Model.Approved == HAVConstants.PENDING; %>
    <div class="clear">&nbsp;</div>
        
    <div class="action-bar bold p-a10 m-btm20 color-4">
        Club Member <%= myClubMemberApproval ? " Approval" : "Edit" %>
    </div>
    <div class="clear">&nbsp;</div>
	    <div class="col-21 m-btm10">
	    	<div class="col-4 center">
	    		<img src="<%= PhotoHelper.ProfilePicture(Model.MemberUser) %>" class="profile lg" />
	    		<div class="clear">&nbsp;</div>
	    	</div>
    		<div class="col-15 m-lft m-rgt fnt-18 bold color-1">
    			<a class="name" href="<%= LinkHelper.Profile(Model.MemberUser) %>"><%= HaveAVoice.Helpers.NameHelper.FullName(Model.MemberUser) %></a>
    			<div class="clear">&nbsp;</div>
                <% string myAction = "Edit"; %>
                <% string myDefaultTitle = Model.Title; %>
                <% bool myDefaultAdminRights = Model.Administrator; %>

                <% if (myClubMemberApproval) { %>
                    <% myAction = "Verdict"; %>
                    <% myDefaultTitle = GroupConstants.DEFAULT_NEW_MEMBER_TITLE; %>
                    <% myDefaultAdminRights = false; %>
                <% } %>

    		    <% if (myClubMemberApproval || Model.Approved == HAVConstants.APPROVED) { %>
                    <% using (Html.BeginForm(myAction, "GroupMember", FormMethod.Post, new { @class = "search-group" })) {%>
                        <%= Html.Hidden("GroupMemberId", Model.Id)%>
                        <%= Html.Hidden("GroupId", Model.GroupId)%>
                        <div class="m-btm10 m-top10">
	                        <label for="Name">Title for user:</label> 
	                        <%= Html.TextBox("Title", myDefaultTitle)%>
                            <%= Html.ValidationMessage("Title", "*")%>
                        </div> 
                        <div class="m-btm10">
                            <label for="Administrator">Admin Rights:</label> 
                            <%= Html.RadioButton("Administrator", true, myDefaultAdminRights)%> <span class="fnt-14"> Yes</span>
                            <%= Html.RadioButton("Administrator", false, !myDefaultAdminRights)%><span class="fnt-14"> No</span>
                        </div>
                        <% if (myClubMemberApproval) { %>
    			            <div class="col-3 center">
    				            <input type="submit" name="approved" value="<%= StatusAction.Approve.ToString() %>" class="button" />
    				            <div class="clear">&nbsp;</div>
    			            </div>
    			            <div class="col-3 center">
    				            <input type="submit" name="approved" value="<%= StatusAction.Deny.ToString() %>" class="button" />
    				            <div class="clear">&nbsp;</div>
    			            </div>
                        <% } else { %>
                            <div class="col-3 center">
    				            <input type="submit" name="submit" value="Submit" class="button" />
    				            <div class="clear">&nbsp;</div>
    			            </div>
                        <% } %>
                    <% } %>
                <% } %>
            </div>
    		<div class="clear">&nbsp;</div>
    	</div>
	<div class="clear">&nbsp;</div>
</div>
</asp:Content>