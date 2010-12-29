<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Login</h2>

    <% using (Html.BeginForm("Login", "Authentication")) {%>
    
    <p>
        <%= Html.Encode(ViewData["ErrorMessage"]) %>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Email">Email:</label>
                <%= Html.TextBox("Email")%>
                <%= Html.ValidationMessage("Email", "*")%>
            </p>
            <p>
                <label for="Description">Password:</label>
                <%= Html.TextBox("Password") %>
                <%= Html.ValidationMessage("Password", "*")%>
            </p>
            <p>
                <label for="RememberMe">Remember me:</label>
                <%= Html.CheckBox("RememberMe") %>         
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>
    <% } %>
    
</asp:Content>
