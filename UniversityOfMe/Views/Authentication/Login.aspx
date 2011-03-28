<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("Login", "Authentication", FormMethod.Post)) { %>
	    <% Html.RenderPartial("Message"); %>
            <div class="editor-label">
                <%: Html.Label("Email") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("Email")%>
                <%: Html.ValidationMessage("Email", "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.Label("Password") %>
            </div>
            <div class="editor-field">
                <%: Html.Password("Password") %>
                <%: Html.ValidationMessage("Password", "*")%>
            </div>

		    <div class="editor-label">
			    <%= Html.CheckBox("RememberMe") %> Remember me
		    </div>

            <p>
                <input type="submit" value="Login" />
            </p>
    <% } %>
</asp:Content>
