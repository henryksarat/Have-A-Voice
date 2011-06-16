<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextbookListModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - All Textbooks
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

 	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="book">Textbooks</span> 
		</div>
        <div class="small center mb25">
            <% using (Html.BeginForm("Search", "TextBook", FormMethod.Post)) {%>
                <div style="display:inline">
                    Search
                    <%= Html.TextBox("SearchString", string.Empty) %>

                    <%= Html.DropDownListFor(model => model.Get().SearchOption, Model.Get().SearchOptions)%>
                </div>
                <div class="ml20" style="display:inline">
                    Order By
                    <%= Html.DropDownListFor(model => model.Get().OrderByOption, Model.Get().OrderByOptions)%>

                    <input type="submit" name="submit" class="btn site" value="Search" /> 
                </div>
            <% } %>
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
            <% foreach (TextBook myTextbook in Model.Get().Textbooks) { %>
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

