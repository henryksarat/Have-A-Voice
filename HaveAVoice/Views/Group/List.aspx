<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupSearchModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Find a Group
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
    	<span class="fnt-16 tint-6 bold">GROUPS</span>
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
		    <div class="push-1 col-19 m-lft issue m-rgt">
		    	<div class="p-a5">
                    <div class="push-1 col-13">
			    	    <h1><a href=""><%= myGroup.Name %></a></h1>
					    <br />
                    
			    	    <%= myGroup.Description %>
			    	    <br />
                        <% string myName = NameHelper.FullName(myGroup.CreatedByUserUser); %>
                        <% string myIssueProfile = LinkHelper.Profile(myGroup.CreatedByUserUser); %>
                        <br />
			    	    <div class="clear">&nbsp;</div>
                    </div>
                    <div class="push-1 col-4 right">
                            <span class="color-1">
                                <% int myGroupMembers = myGroup.GroupMembers.Where(gm => !gm.Deleted).Where(gm => gm.Approved == HAVConstants.APPROVED).Count<GroupMember>(); %>
                                Members: <%= myGroupMembers %><br />
                                <% int myGroupBoardCount = myGroup.GroupBoards.Count; %>
                                <% DateTime myDateTime = myGroup.GroupBoards.Count > 0 ? myGroup.GroupBoards.Max(gb => gb.DateTimeStamp); %>
                                <% if(myDateTime != null) { %>
                                    Last Group Post: myDateTime
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
				
			<div class="push-1 col-3">
				<div class="p-a5">
					<div class="date-tile">
						<span>
							<%= DateHelper.ConvertToLocalTime(myGroup.DateTimeStamp).ToString("MMM").ToUpper() %>
						</span> <%= DateHelper.ConvertToLocalTime(myGroup.DateTimeStamp).ToString("dd").ToUpper()%>
					</div>
				</div>
			</div>
		    
		    <div class="clear">&nbsp;</div>
		</div>
	<% } %>
</div>






<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>
<% using (Html.BeginForm("List", "Group", FormMethod.Post)) {%>
    <div>
        <%= Html.TextBox("searchTerm", string.Empty) %>
    </div>

    <div> 
	    <label for="searchBy">Search By</label> 
        <%= Html.DropDownList("searchBy", Model.SearchByOptions)%>
    </div> 

    <div> 
	    <label for="orderBy">Order By</label> 
        <%= Html.DropDownList("orderBy", Model.OrderByOptions)%>
    </div> 
    <div>
		<input type="submit" name="submit" value="Submit" /> 
	</div> 
<% } %>
<% foreach (HaveAVoice.Models.Group myGroup in Model.SearchResults) { %>    
    Name: <a href="/Group/Details/<%= myGroup.Id %>"><%= myGroup.Name %></a>
    Zip Code Tags: <%= string.Join(",", myGroup.GroupZipCodeTags.Select(z => z.ZipCode.ToString())) %>
    Keyword Tags: <%= string.Join(",", myGroup.GroupTags.Select(k => k.Tag)) %>
    State Tags: <%= string.Join(",", myGroup.GroupCityStateTags.Select(s => s.State)) %>
    City Tags: <%= string.Join(",", myGroup.GroupCityStateTags.Select(c => c.City))%>
    <br />
<% } %>
</asp:Content>