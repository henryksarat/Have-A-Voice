<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ForgotPassword
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ForgotPassword</h2>
    <p>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    </p>

    <% using (Html.BeginForm("ForgotPassword", "User")) {%>

    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
        <fieldset>
            <legend>Fields</legend>
                <p>
                    Please enter your email address.
                </p>

            <p>
                <label for="Email">Email:</label>
                <%= Html.TextBox("Email")%>
                <%= Html.ValidationMessage("Email", "*")%>
            </p>
            
            <p>
                <button name="button" value="submit">Submit</button>
            </p>
        </fieldset>
    <% } %>

</asp:Content>
