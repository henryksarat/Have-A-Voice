<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Class>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold class">Classes you are in</span>
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.ClassWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (Class myClass in Model.Take<Class>(5)) { %>
        <li>
            <% int myTotalReviews = myClass.ClassReviews.Count; %>
			<div class="rating">
                <% if (myTotalReviews > 0) { %>
					<%= StarHelper.AveragedFiveStarImages(myClass.ClassReviews.Sum(r => r.Rating), myTotalReviews) %>
					<span class="gray tiny">(<%= myTotalReviews %> ratings)</span> 
                <% } else { %>
                    There are no reviews yet    
                <% } %>
				<p class="pt7 lightgray">Board Posts: <%= myClass.ClassBoards.Where(c => !c.Deleted).Count<ClassBoard>() %></p> 
            </div> 
			<a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= ClassHelper.CreateClassString(myClass) %></a><br /> 
			<span class="gray"><%= TextShortener.Shorten(myClass.Title, 20) %></span><br /> 
			<span class="gold"><%= myClass.AcademicTerm.DisplayName %> <%= myClass.Year %></span>                     
        </li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Class", null, new { @class="add-new-cross" })%>
</div>
<a class="add-new" href="<%= URLHelper.SearchAllClasses() %>">Search Classes to Enroll In</a>
<%= Html.ActionLink("View All", "List", "Class", null, new { @class="view-all" })%>