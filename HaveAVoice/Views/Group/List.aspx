<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupSearchModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Find a Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

<div class="col-24">
    <div class="spacer-30">&nbsp;</div>
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>
    <% using (Html.BeginForm("Search", "Group", FormMethod.Post, new { @class = "search-group" })) { %>
        <%= Html.Hidden("showMyGroupsOnly", Model.MyGroups) %>
        <div class="col-24 center m-btm25">
	        <%= Html.TextBox("SearchTerm")%>
            <label for="SearcyBy">Search By:</label>
            <%= Html.DropDownList("SearchBy", Model.SearchByOptions)%>
            <label for="OrderBy">Order By:</label>
            <%= Html.DropDownList("OrderBy", Model.OrderByOptions)%>
            <input type="submit" name="submit" value="Search" class="button" /> 
	        <div class="clear">&nbsp;</div>
	    </div>
    <% } %>
    
    <div class="clear">&nbsp;</div>
    
    <div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold">GROUPS <%= Model.MyGroups ? " I'M IN" : "" %></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%= Html.ActionLink("CREATE NEW", "Create", "Group" , null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>
    

	<% foreach (HaveAVoice.Models.Group myGroup in Model.SearchResults) { %>
	    <div class="issue-container m-btm10">            
		    <div class="push-3 col-19 m-lft issue m-rgt">
		    	<div class="p-a5">
                    <div class="push-1 col-11">
			    	    <h1><a href="/Group/Details/<%= myGroup.Id %>"><%= myGroup.Name %></a></h1>
					    <br />
                    
			    	    <%= myGroup.Description %>
                        <br />
			    	    <div class="clear">&nbsp;</div>
                    </div>
                    <div class="push-1 col-6 right">
                            <span class="color-1">
                                <% int myGroupMembers = myGroup.GroupMembers.Where(gm => !gm.Deleted).Where(gm => !gm.OldRecord).Where(gm => gm.Approved == HAVConstants.APPROVED).Count<GroupMember>(); %>
                                Members: <%= myGroupMembers %><br />
                                <% int myGroupBoardCount = myGroup.GroupBoards.Count; %>
                                <% DateTime myDateTime = myGroup.GroupBoards.Count > 0 ? myGroup.GroupBoards.Max(gb => gb.DateTimeStamp) : DateTime.UtcNow; %>
                                <% if (GroupHelper.IsAdmin(myUserInfo.Details, myGroup.Id)) { %>
                                    You are an admin<br />
                                    <% if(!myGroup.Active) { %>
                                        This group is NOT active
                                    <% } %>
                                <% } %>
                                <% if (myGroupBoardCount > 0) { %>
                                    Posts to board: <%= myGroupBoardCount %><br />
                                    Last Group Post: <%= LocalDateHelper.ToLocalTime(myDateTime) %>
                                <% } %>
                            </span>
                    </div>
		    	</div>
		    		
                <div class="p-a5">
			        <div class="push-4 col-3 center">
			            <span class="color-1">
			            </span>
			        </div>
			            
	                <div class="clear">&nbsp;</div>
			    </div>
		    </div>		    
		    <div class="clear">&nbsp;</div>
		</div>
	<% } %>
</div>
</asp:Content>