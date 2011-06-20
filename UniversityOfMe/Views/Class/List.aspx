<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Class>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - All Classes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="banner black full red-top small"> 
			<span class="class">CLASSES</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th>Class Code</th>
                <th>Class Title</th>
                <th>Academic Term</th>
                <th>Year</th>
                <th>Number of Board Posts</th>
                <th>Number of Reviews</th>
                <th>Average Review Score</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (Class myClass in Model.Get()) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassCode %></a></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= TextShortener.Shorten(myClass.ClassTitle, 20) %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.AcademicTerm.DisplayName %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.Year %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassBoards.Count %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassReviews.Count %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassReviews.Count == 0 ? "NA" : (myClass.ClassReviews.Sum(cr=>cr.Rating) / myClass.ClassReviews.Count).ToString() %></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

