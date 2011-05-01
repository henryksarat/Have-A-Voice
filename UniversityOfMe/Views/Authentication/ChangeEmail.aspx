<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.Generic.Models.StringModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ChangeEmail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ChangeEmail</h2>

    <% using (Html.BeginForm("ChangeEmail", "Authentication", FormMethod.Post)) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <%= Html.Hidden("NewEmailHash", Model.Value) %>
        
            
        <div class="editor-label">
            <%: Html.Label("Old Email") %>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("OldEmail") %>
            <%: Html.ValidationMessage("OldEmail", "*")%>
        </div>
                        
        <p>
            <input type="submit" value="Change" />
        </p>

    <% } %>
</asp:Content>
