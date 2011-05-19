<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<GeneralPosting>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

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
