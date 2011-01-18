<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Enter an email of an authority to send to</h2>
    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>

    <%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>

	<% using (Html.BeginForm("Create", "AuthorityVerification")) { %>
		<%= Html.TextBox("Email")%>
        <%= Html.ValidationMessage("Email", "*")%>
		<div class="clear">&nbsp;</div>
		<div class="right m-top10">
			<input type="submit" value="Send" />
		</div>
	<% } %>
</asp:Content>
