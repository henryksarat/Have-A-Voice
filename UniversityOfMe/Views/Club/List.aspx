<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Club>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>
    
 	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="club">Clubs</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th>Name</th>
		        <th>Number of Members</th>
                <th>Last Board Post</th>
                <th>Last Board Post Date</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (Club myClub in Model.Get()) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <% ClubBoard myLastClubBoard = myClub.ClubBoards.Count > 0 ? myClub.ClubBoards.OrderByDescending(b => b.DateTimeStamp).First<ClubBoard>() : null; %>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClubUrl(myClub) %>"><%= myClub.Name %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClubUrl(myClub) %>"><%= myClub.ClubMembers.Count %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClubUrl(myClub) %>"><%= myLastClubBoard == null ? "NA" : TextShortener.Shorten(myLastClubBoard.Message, 20) %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildClubUrl(myClub) %>"><%= myLastClubBoard == null ? "NA" : LocalDateHelper.ToLocalTime(myLastClubBoard.DateTimeStamp)%></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

