<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupMember>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="col-24 m-btm30">
    <div class="spacer-30">&nbsp;</div>

	<% Html.RenderPartial("Message"); %>
	<% Html.RenderPartial("Validation"); %>
    <div class="clear">&nbsp;</div>
        
    <div class="action-bar bold p-a10 m-btm20 color-4">
        Pending Friend Requests
    </div>
    <div class="clear">&nbsp;</div>
	    <div class="col-21 m-btm10">
	    	<div class="col-2 center">
	    		<img src="/Photos/no_profile_picture.jpg" class="profile sm" />
	    		<div class="clear">&nbsp;</div>
	    	</div>
    		<div class="col-15 m-lft m-rgt fnt-12 bold color-1">
    			<%= HaveAVoice.Helpers.NameHelper.FullName(Model.MemberUser) %>
    			<div class="clear">&nbsp;</div>
    		</div>
            <% using (Html.BeginForm("Verdict", "GroupMember", FormMethod.Post, new { @class = "search-group" })) {%>
                <% if (Model.Approved == HAVConstants.PENDING) { %>
                    <%= Html.Hidden("GroupMemberId", Model.Id)%>
                    <%= Html.Hidden("GroupId", Model.GroupId)%>
                    <div>
	                    <label for="Name">Title for user:</label> 
	                    <%= Html.TextBox("Title", GroupConstants.DEFAULT_NEW_MEMBER_TITLE)%>
                        <%= Html.ValidationMessage("Title", "*")%>
                    </div> 
                    <div>
                        <label for="Administrator">Admin Rights:</label> 
                        <%= Html.RadioButton("Administrator", true)%> Yes
                        <%= Html.RadioButton("Administrator", false, true)%> No
                    </div>
    			    <div class="col-2 center">
    				    <button name="approved" value="true" class="search-group">Approve</button>
    				    <div class="clear">&nbsp;</div>
    			    </div>
    			    <div class="col-2 center">
    				    <button name="approved" value="false" class="search-group">Deny</button>
    				    <div class="clear">&nbsp;</div>
    			    </div>
                <% } %>
            <% } %>
    		<div class="clear">&nbsp;</div>
    	</div>
	<div class="clear">&nbsp;</div>
</div>
</asp:Content>