<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<GeneralPosting>>" %>
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
			<span class="general">General Postings</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th>Title</th>
		        <th>Number of Replies</th>
                <th>Last Post</th>
                <th>End Post Date</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (GeneralPosting myGeneralPosting in Model.Get().OrderByDescending(g => g.GeneralPostingReplies.OrderByDescending(r => r.DateTimeStamp))) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <% GeneralPostingReply myLastReply = myGeneralPosting.GeneralPostingReplies.OrderByDescending(r => r.DateTimeStamp).First<GeneralPostingReply>(); %>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildGeneralPostingsUrl(myGeneralPosting) %>"><%= myGeneralPosting.Title%></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildGeneralPostingsUrl(myGeneralPosting) %>"><%= myGeneralPosting.GeneralPostingReplies.Count %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildGeneralPostingsUrl(myGeneralPosting) %>"><%= TextShortener.Shorten(myLastReply.Reply, 20) %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildGeneralPostingsUrl(myGeneralPosting) %>"><%= LocalDateHelper.ToLocalTime(myLastReply.DateTimeStamp)%></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

