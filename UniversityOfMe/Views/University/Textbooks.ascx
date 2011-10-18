<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<TextBook>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold book">Latest Textbooks</span> 
</div>

<ul> 
    <% foreach (TextBook myTextBook in Model.Take<TextBook>(25)) { %>
        <li>
			<a class="itemlinked" href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>"><%= TextShortener.Shorten(myTextBook.BookTitle, 40) %></a>
			<span class="darkgray"> <%= string.IsNullOrEmpty(myTextBook.ClassSubject + myTextBook.ClassCourse) ? string.Empty : "(" + myTextBook.ClassSubject + myTextBook.ClassCourse + ")"%></span> 
        </li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "TextBook", null, new { @class="add-new-cross" })%>
</div>

<%= Html.ActionLink("Sell Book", "Create", "TextBook", null, new { @class="add-new" })%>
<a class="view-all" href="<%= URLHelper.SearchTextbooksByClass() %>">Search By Class</a>
<a class="view-all" href="<%= URLHelper.SearchTextbooksByTitle() %>">Search By Title</a>