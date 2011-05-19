<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateClassModel>>" %>
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
                <%: Html.Label("Please provide the academic term and year for the class:") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.Get().AcademicTermId, Model.Get().AcademicTerms)%>
                <%: Html.ValidationMessageFor(model => model.Get().AcademicTermId, "*")%>

                <%: Html.DropDownListFor(model => model.Get().Year, Model.Get().Years)%>
                <%: Html.ValidationMessageFor(model => model.Get().Year, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Class Code") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().ClassCode)%>
                <%: Html.ValidationMessageFor(model => model.Get().ClassCode, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Class Title") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().ClassTitle)%>
                <%: Html.ValidationMessageFor(model => model.Get().ClassTitle, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Details") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Details)%>
                <%: Html.ValidationMessageFor(model => model.Get().Details, "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
