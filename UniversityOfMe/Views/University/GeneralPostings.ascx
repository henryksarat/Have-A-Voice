<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<GeneralPosting>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="nomral bold post">General Postings</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.GeneralPostingsWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (GeneralPosting myGeneralPosting in Model.Take<GeneralPosting>(5)) { %>                           
		<li> 
			<a class="itemlinked" href="/<%= myGeneralPosting.UniversityId %>/GeneralPosting/Details/<%= myGeneralPosting.Id %>"><%= TextShortener.Shorten(myGeneralPosting.Title, 20) %></a><br />
			<span class="tiny darkgray">Posts: <%= myGeneralPosting.GeneralPostingReplies.Count %></span><br /> 
			<span class="tiny darkgray">Last post: <%= myGeneralPosting.GeneralPostingReplies.Count != 0 ? LocalDateHelper.ToLocalTime(myGeneralPosting.GeneralPostingReplies.OrderByDescending(gp => gp.DateTimeStamp).First().DateTimeStamp) : LocalDateHelper.ToLocalTime(myGeneralPosting.DateTimeStamp)%></span> 
		</li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "GeneralPosting", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "GeneralPosting", null, new { @class="add-new" })%>
<%= Html.ActionLink("View All", "List", "GeneralPosting", null, new { @class="view-all" })%>