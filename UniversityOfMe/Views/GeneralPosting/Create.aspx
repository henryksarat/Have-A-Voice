<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
    <% using (Html.BeginForm()) {%>
        <fieldset>
            <div class="editor-label">
                <%: Html.Label("Title")%>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("Title")%>
                <%: Html.ValidationMessage("Title", "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Body")%>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("Body")%>
                <%: Html.ValidationMessage("Body", "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>
    <% } %>
</asp:Content>
