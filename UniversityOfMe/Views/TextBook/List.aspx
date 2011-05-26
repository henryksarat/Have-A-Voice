<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<TextBook>>" %>
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
			<span class="book">Textbooks</span> 
		</div> 
					
		<table class="listing row"> 
            <tr class="heading">
		        <th></th>
                <th>Title</th>
                <th>Class Code</th>
                <th>Edition</th>
                <th>Price</th>
                <th>Posted</th>
            </tr>
            <% bool mySwitch = false; %>
            <% foreach (TextBook myTextbook in Model.Get().OrderByDescending(t => t.DateTimeStamp)) { %>
                <% if (mySwitch) { %>
                    <% mySwitch = false; %>
                    <tr class="small center alternative">
                <% } else { %>
                    <% mySwitch = true; %>
                    <tr class="small center">
                <% } %>
                    <% string myBuySellCssClass = myTextbook.BuySell.Equals("B") ? "buy" : "sell"; %>
                    <td><a class="<%= myBuySellCssClass %>" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"></a></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"><%= myTextbook.BookTitle %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"><%= myTextbook.ClassCode %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"><%= myTextbook.Edition %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"><%= myTextbook.Price %></td>
                    <td><a class="itemlinked" href="<%= URLHelper.BuildTextbookUrl(myTextbook) %>"><%= DateHelper.ToLocalTime(myTextbook.DateTimeStamp) %></td>
                </tr>
            <% } %>
		</table> 		
	</div> 
</asp:Content>

