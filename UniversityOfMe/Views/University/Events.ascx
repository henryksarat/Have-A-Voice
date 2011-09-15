<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Event>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold event">Latest Events on Campus</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.EventWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (Event myEvent in Model.Take<Event>(5)) { %>
		<li> 
			<a class="itemlinked" href="<%= URLHelper.EventUrl(myEvent) %>"><%= TextShortener.Shorten(myEvent.Title, 25) %></a>
			<div class="rating darkgray"> 
				<%= LocalDateHelper.ToLocalTime(myEvent.StartDate) %>
			</div> 
		</li> 
    <% } %>
</ul>
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Event", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "Event", null, new { @class="add-new" })%>
<%= Html.ActionLink("View All", "List", "Event", null, new { @class="view-all" })%>