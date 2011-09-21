<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.Generic.Models.StringModel>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Resend Activation Email
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Request to have your activation email resent")%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords("resend activation email")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="twelve"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form create">
	        <div class="banner black full small red-top"> 
		        RESEND ACTIVATION EMAIL
	        </div> 
                <% Html.RenderPartial("Validation"); %>
                <div style="margin-left:20%; margin-right: 20%">
                    <% if(Model.Value.Equals("1")) { %>
                        It appears that email is already registered but not activated. Enter your email address and we will resend your activation email so you can activate your account. 
                    <% } else if (Model.Value.Equals("2")) { %>
                        Enter your email address and we will resend your activation email so you can activate your account. 
                    <% } else if (Model.Value.Equals("3")) { %>
                        It appears that the account wasn't activated. Enter your email address and we will resend your activation email so you can activate your account.
                    <% } %>
                </div>
		        <% using (Html.BeginForm("Activation", "User", FormMethod.Post, new { @class = "form-normal" })) { %>
                    <%= Html.Hidden("Message", Model.Value) %>
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
