<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Professor>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - All Professors
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

 	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="professor">Professors</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th>Professor Name</th>
                <th>Number of Reviews</th>
                <th>Average Score</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (Professor myProfessor in Model.Get()) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildProfessorUrl(myProfessor) %>"><%= myProfessor.FirstName + " " + myProfessor.LastName%></a></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildProfessorUrl(myProfessor) %>"><%= myProfessor.ProfessorReviews.Count%></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildProfessorUrl(myProfessor) %>"><%= myProfessor.ProfessorReviews.Count == 0 ? "NA" : (myProfessor.ProfessorReviews.Sum(cr => cr.Rating) / myProfessor.ProfessorReviews.Count).ToString()%></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

