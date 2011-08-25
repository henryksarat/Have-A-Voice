<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<UserStatus>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="wp90">
    <span class="normal bold bubble">What People Are Doing</span> 
    <span class="frgt">
        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.StatusWidget }, new { @class = "disable-feature" })%>
    </span>
</div>

<ul> 
    <% foreach (UserStatus myUserStatus in Model.Take<UserStatus>(5)) { %>
        <li>
            <a class="itemlinked" href="<%= URLHelper.ProfileUrl(myUserStatus.User) %>"><%= NameHelper.FullName(myUserStatus.User) %></a>
            <span class="black">is <%= myUserStatus.Status %> at <%= LocalDateHelper.ToLocalTime(myUserStatus.DateTimeStamp) %> </span>
        </li>                         
    <% } %>
</ul> 

<%= Html.ActionLink("View All Latest", "List", "UserStatus", null, new { @class="view-all" })%>