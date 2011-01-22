<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.StringWrapper>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Verify
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Enter the email that this token was sent to</h2>
    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>

    <%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>
    <%= Model.Value %>
	<% using (Html.BeginForm("Verify", "AuthorityVerification")) { %>
    <%= Html.Hidden("Token", Model.Value) %>
		<%= Html.TextBox("Email")%>
        <%= Html.ValidationMessage("Email", "*")%>
		<div class="clear">&nbsp;</div>
		<div class="right m-top10">
			<input type="submit" value="Verify" />
		</div>
	<% } %>

</asp:Content>
