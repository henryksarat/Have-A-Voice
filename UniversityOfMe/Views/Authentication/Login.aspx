<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="twelve"> 
        <div class="create-feature-form create">
	        <div class="banner black full small red-top"> 
		        LOGIN
	        </div> 
	        <p class="p20">
                    <% Html.RenderPartial("Message"); %>
                    <% Html.RenderPartial("Validation"); %>
		            <% using (Html.BeginForm("Login", "Authentication", FormMethod.Post, new { @class = "form-normal" })) { %>
                        <div class="input">
                            <%: Html.Label("Email") %>
                            <%: Html.TextBox("Email")%>
                            <%: Html.ValidationMessage("Email", "*")%>
                        </div>
            
                        <div class="input">
                            <%: Html.Label("Password") %>
                            <%: Html.Password("Password") %>
                            <%: Html.ValidationMessage("Password", "*")%>
                        </div>

		                <div class="input">
                            <span class="empty-label">&nbsp;</span>
			                <%= Html.CheckBox("RememberMe") %> Remember me
		                </div>

			            <div> 
                            <span class="empty-label">&nbsp;</span>
				            <input type="submit" name="submit" class="btn" value="Login" /> 
			            </div> 
                    <% } %>
	        </p> 
        </div>
    </div> 
</asp:Content>
