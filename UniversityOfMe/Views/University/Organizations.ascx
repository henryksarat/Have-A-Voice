<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Club>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold club">Most Active Organizations</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.OrganizationWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (Club myClub in Model.Take<Club>(5)) { %>
        <li>
            <a class="itemlinked" href="<%= URLHelper.BuildOrganizationUrl(myClub) %>"><%= myClub.Name %></a> - <span class="gold"><%= myClub.ClubTypeDetails.DisplayName %>
        </li>                         
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Club", null, new { @class="add-new-cross" })%>
</div>
<%= Html.ActionLink("Create New", "Create", "Club", null, new { @class = "add-new" })%>
<%= Html.ActionLink("View All", "List", "Club", null, new { @class="view-all" })%>