<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.Generic.Models.StringModel>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forget Password Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="twelve"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form create">
	        <div class="banner black full small red-top"> 
		        CHANGE PASSWORD
	        </div> 
                <% Html.RenderPartial("Validation"); %>
                <div style="margin-left:20%">
                    Use the form below to change your password. New passwords are required to be a minimum of 6 characters in length.
                </div>
		        <% using (Html.BeginForm("Process", "Password", FormMethod.Post, new { @class = "form-normal" })) { %>
                    <%= Html.Hidden("ForgotPasswordHash", Model.Value) %>
                    <div class="editor-label">
                        <%: Html.Label("Email") %>
                    </div>
                    
                    <div class="editor-field">
                        <%: Html.TextBox("Email")%>
                        <%: Html.ValidationMessage("Email", "*", new { @class = "req" })%>
                    </div>

                    <div class="input">
                    </div>

                    <div class="editor-label">
                        <%: Html.Label("New Password") %>
                    </div>
                    
                    <div class="editor-field">
                        <%: Html.Password("Password")%>
                        <%: Html.ValidationMessage("Password", "*", new { @class = "req" })%>
                    </div>

                    <div class="input">
                    </div>

                    <div class="editor-label">
                        <%: Html.Label("Retyped Password") %>
                    </div>
                    
                    <div class="editor-field">
                        <%: Html.Password("RetypedPassword")%>
                        <%: Html.ValidationMessage("RetypedPassword", "*", new { @class = "req" })%>
                    </div>

                    <div class="input">
                    </div>

			        <div>
                        <span class="empty-label">&nbsp;</span>
				        <input type="submit" name="submit" class="btn" value="Submit" /> 
			        </div> 
                <% } %>
        </div>
    </div> 
</asp:Content>
