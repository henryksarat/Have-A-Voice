<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateClassModel>" %>

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
                <%: Html.Label("Please provide the academic term and year for the class:") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.AcademicTermId, Model.AcademicTerms)%>
                <%: Html.ValidationMessageFor(model => model.AcademicTermId, "*")%>

                <%: Html.DropDownListFor(model => model.Year, Model.Years)%>
                <%: Html.ValidationMessageFor(model => model.Year, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Class Code") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ClassCode)%>
                <%: Html.ValidationMessageFor(model => model.ClassCode, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Class Title") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ClassTitle)%>
                <%: Html.ValidationMessageFor(model => model.ClassTitle, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Details") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Details)%>
                <%: Html.ValidationMessageFor(model => model.Details, "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
