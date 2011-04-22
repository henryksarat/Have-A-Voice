<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <fieldset>
            <div class="editor-label">
                <%: Html.Label("First Name") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("FirstName")%>
                <%: Html.ValidationMessage("FirstName", "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Last Name") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("LastName")%>
                <%: Html.ValidationMessage("LastName", "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Optional Professor Image") %>
            </div>
            <div class="editor-field">
                <input type="file" id="ProfessorImage" name="ProfessorImage" size="23" />
                <%: Html.ValidationMessage("ProfessorImage", "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
