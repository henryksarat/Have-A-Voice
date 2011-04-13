<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.EventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.Label("Event privacy option") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.EventPrivacyOption, Model.EventPrivacyOptions)%>
                <%: Html.ValidationMessageFor(model => model.EventPrivacyOption, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Title) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Title)%>
                <%: Html.ValidationMessageFor(model => model.Title)%>
            </div>
            
            <div class="editor-label">
                <%: Html.Label("Start Date") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.StartDate) %>
                <%: Html.ValidationMessageFor(model => model.StartDate)%>
            </div>
            
            <div class="editor-label">
                <%: Html.Label("End Date") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.EndDate)%>
                <%: Html.ValidationMessageFor(model => model.EndDate)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Information) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Information) %>
                <%: Html.ValidationMessageFor(model => model.Information) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>

