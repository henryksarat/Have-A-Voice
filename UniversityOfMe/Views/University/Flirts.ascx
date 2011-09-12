<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<AnonymousFlirt>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold flirt">Anonymous Flirts</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.FlirtWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (AnonymousFlirt myFlirt in Model.Take<AnonymousFlirt>(5)) { %>
		<li>
            <span class="black"> 
			    <%= myFlirt.Adjetive %>
                <%= myFlirt.SomethingDelicious %>
                <%= myFlirt.Animal %>, that
                <%= myFlirt.HairColor.Equals("Dunno") ? string.Empty : myFlirt.HairColor %> Haired
                <%= myFlirt.Gender %> 
                <%= string.IsNullOrEmpty(myFlirt.Location) ? string.Empty : " i saw at " + myFlirt.Location %> -
                <%= myFlirt.Post %>
            </span>
		</li> 
    <% } %>
</ul>
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Flirt", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "Flirt", null, new { @class="add-new" })%>
<%= Html.ActionLink("View All", "List", "Flirt", null, new { @class="view-all" })%>