<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Professor>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold professor">Latest Professor Rating</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.ProfessorWidget }, new { @class = "disable-feature" })%>
    </span>
</div>
<ul> 
    <% foreach (Professor myProfessor in Model.Take<Professor>(5)) { %>
        <li>
            <% string myProfessorNameUrl = URLHelper.ToUrlFriendly(myProfessor.FirstName + " " + myProfessor.LastName); %>
            <a class="itemlinked" href="<%= URLHelper.BuildProfessorUrl(myProfessor) %>"><%= myProfessor.FirstName + " " +  myProfessor.LastName %></a>                         
            <% int myTotalReviews = myProfessor.ProfessorReviews.Count; %>
			<div class="rating"> 
            <% if (myTotalReviews > 0) { %>
                <%= StarHelper.AveragedFiveStarImages(myProfessor.ProfessorReviews.Sum(r => r.Rating), myTotalReviews) %>
				<span class="gray tiny">(<%= myTotalReviews%> ratings)</span> 
            <% } else { %>
				<span class="gray tiny">No reviews yet</span> 
            <% } %>
            </div>
        </li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Professor", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "Professor", null, new { @class="add-new" })%>
<%= Html.ActionLink("View All", "List", "Professor", null, new { @class="view-all" })%>