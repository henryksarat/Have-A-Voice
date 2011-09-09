<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forget Password Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="twelve"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form create">
	        <div class="banner black full small red-top"> 
		        PASSWORD REQUEST
	        </div> 
                <% Html.RenderPartial("Validation"); %>
                <div style="margin-left:20%">
                    Enter your email address and we will send you an email with instructions on how to change your password.
                </div>
		        <% using (Html.BeginForm("Request", "Password", FormMethod.Post, new { @class = "form-normal" })) { %>
                    <div class="editor-label">
                        <%: Html.Label("Email") %>
                    </div>
                    
                    <div class="editor-field">
                        <%: Html.TextBox("Email")%>
                        <%: Html.ValidationMessage("Email", "*", new { @class = "req" })%>
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
