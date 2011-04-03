<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateProfessorModel>" %>

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
                <%: Html.TextBoxFor(model => model.FirstName)%>
                <%: Html.ValidationMessageFor(model => model.FirstName, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Last Name") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.LastName)%>
                <%: Html.ValidationMessageFor(model => model.LastName, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("University") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.UniversityId, Model.Universities)%>
                <%: Html.ValidationMessageFor(model => model.UniversityId, "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
