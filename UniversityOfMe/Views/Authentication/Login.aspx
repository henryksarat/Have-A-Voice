<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="twelve"> 
	    <div class="banner black full small red-top"> 
		    LOGIN
	    </div> 
	    <p class="p20"> 
                <% Html.RenderPartial("Message"); %>
                <% Html.RenderPartial("Validation"); %>
		        <% using (Html.BeginForm("Login", "Authentication", FormMethod.Post)) { %>
                        <div class="input">
                            <%: Html.Label("Email") %>
                        </div>
                        <div class="input">
                            <%: Html.TextBox("Email")%>
                            <%: Html.ValidationMessage("Email", "*")%>
                        </div>
            
                        <div class="input">
                            <%: Html.Label("Password") %>
                        </div>
                        <div class="input">
                            <%: Html.Password("Password") %>
                            <%: Html.ValidationMessage("Password", "*")%>
                        </div>

		                <div class="editor-label">
			                <%= Html.CheckBox("RememberMe") %> Remember me
		                </div>

			            <div class="input half"> 
				            <input type="submit" name="submit" class="btn" value="Login" /> 
			            </div> 
                <% } %>
	    </p> 
    </div> 
</asp:Content>
