<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<MarketplaceItem>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold cart">Latest Marketplace Items</span> 
</div>

<ul> 
    <% foreach (MarketplaceItem myItem in Model.Take<MarketplaceItem>(25)) { %>
        <li>
			<a class="itemlinked" href="/<%= myItem.UniversityId %>/Marketplace/Details/<%= myItem.Id %>"><%= TextShortener.Shorten(myItem.Title, 20) %></a>
			<span class="darkgray"> <%= myItem.ItemTypeId %></span> 
            <span class="darkgray"> @ <%= UniversityOfMe.Helpers.Format.MoneyFormatHelper.Format(myItem.Price) %></span> 
        </li>
    <% } %>
</ul> 
<div class="flft mr9">
    <%= Html.ActionLink("+", "Create", "Marketplace", null, new { @class = "add-new-cross" })%>
</div>
<%= Html.ActionLink("Add Listing", "Create", "Marketplace", null, new { @class = "add-new" })%>