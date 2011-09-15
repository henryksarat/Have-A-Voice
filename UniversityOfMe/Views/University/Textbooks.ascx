<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<TextBook>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold book">Buy Textbooks</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.TextbookWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (TextBook myTextBook in Model.Take<TextBook>(5)) { %>
        <li>
			<a class="itemlinked" href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>"><%= TextShortener.Shorten(myTextBook.BookTitle, 40) %></a>
			<span class="darkgray">( <%= myTextBook.ClassCode %>)</span> 
        </li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "TextBook", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "TextBook", null, new { @class="add-new" })%>
<%= Html.ActionLink("View All", "List", "TextBook", null, new { @class="view-all" })%>