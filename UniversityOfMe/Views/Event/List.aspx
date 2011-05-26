<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Event>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>


 	<div class="eight last"> 
		<div class="banner black full red-top small"> 
			<span class="event">Events</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th>Title</th>
                <th>Start Date</th>
                <th>End Date</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (Event myEvent in Model.Get().OrderByDescending(t => t.StartDate)) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildEventUrl(myEvent) %>"><%= myEvent.Title %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildEventUrl(myEvent) %>"><%= DateHelper.ToLocalTime(myEvent.StartDate) %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildEventUrl(myEvent) %>"><%= DateHelper.ToLocalTime(myEvent.EndDate)%></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

